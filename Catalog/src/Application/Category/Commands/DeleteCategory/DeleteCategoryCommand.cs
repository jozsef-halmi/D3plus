using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.Categorys.Commands.DeleteCategory;

public record DeleteCategoryCommand : IRequest<int>
{
    public int CategoryId { get; init; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Categories.FirstOrDefault(c => c.Id == request.CategoryId);
        if (entity == null)
            throw new NotFoundException();

        _context.Categories.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
