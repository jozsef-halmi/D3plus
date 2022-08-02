using AutoMapper;
using Catalog.Application.Categorys.Queries.GetCategory;
using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.TodoLists.Queries.GetCategory;

public record GetCategoryQuery : IRequest<CategoryDto>
{
    public int Id { get; set; }
}

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var entity = _context.Categories.FirstOrDefault(c => c.Id == request.Id);
        if (entity == null)
            throw new NotFoundException();

        return _mapper.Map<CategoryDto>(entity);
    }
}
