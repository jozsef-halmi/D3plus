using Catalog.Application.Categorys.Queries.GetCategories;

namespace Catalog.GraphQL.GraphQL.Types;

public class CategoryList
{
    public IEnumerable<CategoryDto> Categories { get; set; }
}
