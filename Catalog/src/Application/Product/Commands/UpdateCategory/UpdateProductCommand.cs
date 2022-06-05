using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<int>
{
    public int ProductId { get; init; }
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

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Products.FirstOrDefault(c => c.Id == request.ProductId);
        if (entity == null)
            throw new NotFoundException();

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.ImageUrl = request.ImageUrl;
        entity.Price = request.Price;
        entity.Amount = request.Amount;
        entity.CategoryId = request.CategoryId;

        _context.Products.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
