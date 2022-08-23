using System.Diagnostics;
using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Outbox;
using MediatR;
using Messaging.Contracts;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Newtonsoft.Json;

namespace Catalog.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<int>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public Uri? ImageUrl { get; init; }
    public int CategoryId { get; init; }
    public decimal Price { get; init; }
    public int Amount { get; init; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly JsonSerializerSettings _jsonSerializerSettings;

    private readonly TelemetryConfiguration _telemetryConfiguration;
    private readonly TelemetryClient _telemetryClient;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
        _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        _telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        _telemetryClient = new TelemetryClient(_telemetryConfiguration);
    }

    public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Products.FirstOrDefault(c => c.Id == request.Id);
        if (entity == null)
            throw new NotFoundException();

        var oldPrice = entity.Price;

        try
        {
            await _context.StartTransaction();

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.ImageUrl = request.ImageUrl;
            entity.Price = request.Price;
            entity.Amount = request.Amount;
            entity.CategoryId = request.CategoryId;

            _context.Products.Update(entity);

            AddIntegrationEvent(entity.Id, oldPrice, entity.Price);

            // test

            var a = _context.OutboxMessages.ToList();
            await _context.SaveChangesAsync(cancellationToken);
            await _context.Commit();
            return entity.Id;
        }
        catch (Exception)
        {
            await _context.Rollback();
            throw;
        }
       
    }

    public void AddIntegrationEvent(int productId, decimal oldPrice, decimal newPrice)
    {
        if (oldPrice == newPrice)
            return;

        var integrationEvent = new ProductPriceChangedIntegrationEvent(productId, newPrice, oldPrice,
            Activity.Current.Context.SpanId.ToString(),
            Activity.Current.ParentSpanId.ToString()
            );
        
        _context.OutboxMessages.Add(new OutboxMessage()
        {
            PublishedDate = null,
            IntegrationEventJson = JsonConvert.SerializeObject(integrationEvent, _jsonSerializerSettings),
        });
    }
}
