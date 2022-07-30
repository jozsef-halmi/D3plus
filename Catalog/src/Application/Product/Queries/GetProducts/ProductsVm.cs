using Catalog.Application.Product.Queries.Common;

namespace Catalog.Application.TodoLists.Queries.GetProducts;

public class ProductsVm
{
    public IList<ProductDto> Products { get; set; } = new List<ProductDto>();
}
