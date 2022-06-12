using Catalog.Application.Common.Models;
using Catalog.Application.Product.Queries.GetProductsWithPagination;

namespace Catalog.Application.TodoLists.Queries.GetProducts;

public class ProductsWithPaginationVm
{
    public PaginatedList<ProductDto> Products { get; set; }
}
