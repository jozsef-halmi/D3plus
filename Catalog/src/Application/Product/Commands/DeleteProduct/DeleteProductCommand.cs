using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Events;
using MediatR;

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

        _context.Products.Remove(entity);

        entity.AddDomainEvent(new ProductDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
