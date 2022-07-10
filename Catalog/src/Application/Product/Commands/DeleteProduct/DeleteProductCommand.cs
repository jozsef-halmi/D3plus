using System.Text.Json;
using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Outbox;
using MediatR;
using Messaging.Contracts;

namespace Catalog.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand : IRequest<int>
{
    public int Id { get; init; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Products.FirstOrDefault(c => c.Id == request.Id);
        if (entity == null)
            throw new NotFoundException();

        try
        {
            await _context.StartTransaction();

            _context.Products.Remove(entity);

            AddIntegrationEvent(entity.Id);

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

    public void AddIntegrationEvent(int productId)
    {
        var integrationEvent = new ProductDeletedIntegrationEvent(productId);

        _context.OutboxMessages.Add(new OutboxMessage()
        {
            PublishedDate = null,
            IntegrationEventType = integrationEvent.GetType().FullName,
            IntegrationEventJson = JsonSerializer.Serialize(integrationEvent)
        });
    }
}
