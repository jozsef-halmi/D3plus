using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Mappings;
using Catalog.Application.Common.Models;
using MediatR;
using ProductDto = Catalog.Application.Product.Queries.GetProductsWithPagination.ProductDto;
namespace Catalog.Application.TodoLists.Queries.GetProductsWithPagination;

public record GetProductsWithPaginationQuery : IRequest<PaginatedList<ProductDto>>
{
    public int? CategoryId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductsWithPaginationQuery, PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
             .Where(x => !request.CategoryId.HasValue || x.CategoryId == request.CategoryId)
             .OrderBy(x => x.Name)
             .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
             .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
