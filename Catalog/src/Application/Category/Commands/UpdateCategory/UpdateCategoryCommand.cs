using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.Categorys.Commands.UpdateCategory;

public record UpdateCategoryCommand : IRequest<int>
{
    public int Id { get; init; }
    public string Name { get; init; }
    public Uri? ImageUrl { get; init; }
    public int? ParentCategoryId { get; init; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Categories.FirstOrDefault(c => c.Id == request.Id);
        if (entity == null)
            throw new NotFoundException();

        entity.Name = request.Name;
        entity.ImageUrl = request.ImageUrl;
        entity.ParentCategoryId = request.ParentCategoryId;

        _context.Categories.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
