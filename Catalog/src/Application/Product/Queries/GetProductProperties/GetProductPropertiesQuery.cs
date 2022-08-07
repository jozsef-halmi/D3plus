using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Product.Queries.Common;
using MediatR;
namespace Catalog.Application.Product.Queries.GetProduct;

public record GetProductPropertiesQuery : IRequest<ProductPropertiesDto>
{
    public int Id { get; init; }
}

public class GetProductPropertiesQueryHandler : IRequestHandler<GetProductPropertiesQuery, ProductPropertiesDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductPropertiesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<ProductPropertiesDto> Handle(GetProductPropertiesQuery request, CancellationToken cancellationToken)
    {
        var product = _context.Products
             .FirstOrDefault(x => x.Id == request.Id);

        if (product == null)
            throw new NotFoundException();

        // Simplification: returning hard coded values 
        return Task.FromResult(new ProductPropertiesDto()
        {
            Id = request.Id,
            Properties = new Dictionary<string, string>()
            {
                { "brand", "Samsung" },
                { "model", "S10" }
            }
        });
    }
}
