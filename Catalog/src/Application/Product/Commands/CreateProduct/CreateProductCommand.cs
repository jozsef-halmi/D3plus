using Catalog.Application.Common.Interfaces;
using Catalog.Application.Products.Exceptions;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<int>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public Uri? ImageUrl { get; init; }
    public int CategoryId { get; init; }
    public decimal Price { get; init; }
    public int Amount { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (!_context.Categories.Any(c => c.Id == request.CategoryId))
            throw new CategoryNotFoundException();

        var entity = new Domain.Entities.Product();

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.ImageUrl = request.ImageUrl;
        entity.Price = request.Price;
        entity.Amount = request.Amount;
        entity.CategoryId = request.CategoryId;

        _context.Products.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
