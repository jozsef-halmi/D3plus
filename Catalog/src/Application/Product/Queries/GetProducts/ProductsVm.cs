using Catalog.Application.Products.Queries.GetProducts;

namespace Catalog.Application.TodoLists.Queries.GetProducts;

public class ProductsVm
{
    public IList<ProductDto> Products { get; set; } = new List<ProductDto>();
}
