using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand : IRequest<int>
{
    public int ProductId { get; init; }
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
        var entity = _context.Products.FirstOrDefault(c => c.Id == request.ProductId);
        if (entity == null)
            throw new NotFoundException();

        _context.Products.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
