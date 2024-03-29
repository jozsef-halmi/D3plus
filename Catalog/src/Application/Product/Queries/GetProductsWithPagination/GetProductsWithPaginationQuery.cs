﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Mappings;
using Catalog.Application.Product.Queries.Common;
using Catalog.Application.TodoLists.Queries.GetProducts;
using MediatR;
namespace Catalog.Application.TodoLists.Queries.GetProductsWithPagination;

public record GetProductsWithPaginationQuery : IRequest<ProductsWithPaginationVm>
{
    public int CategoryId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductsWithPaginationQuery, ProductsWithPaginationVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductsWithPaginationVm> Handle(GetProductsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return new ProductsWithPaginationVm()
        {
            Products = await _context.Products
             .Where(x => x.CategoryId == request.CategoryId)
             .OrderBy(x => x.Name)
             .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
             .PaginatedListAsync(request.PageNumber, request.PageSize)
        };
    }
}
