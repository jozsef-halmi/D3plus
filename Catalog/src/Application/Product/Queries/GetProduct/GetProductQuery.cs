using AutoMapper;
using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Product.Queries.Common;
using MediatR;
namespace Catalog.Application.Product.Queries.GetProduct;

public record GetProductQuery : IRequest<ProductDto>
{
    public int Id { get; init; }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = _context.Products
             .FirstOrDefault(x => x.Id == request.Id);

        if (product == null)
            throw new NotFoundException();

        return Task.FromResult(_mapper.Map<ProductDto>(product));
    }
}
