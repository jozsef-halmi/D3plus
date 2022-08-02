using Catalog.Application.Common.Interfaces;
using Catalog.Application.Products.Exceptions;
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

        var entity = new Domain.Entities.Product
        {
            Name = request.Name,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            Price = request.Price,
            Amount = request.Amount,
            CategoryId = request.CategoryId
        };

        _context.Products.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
