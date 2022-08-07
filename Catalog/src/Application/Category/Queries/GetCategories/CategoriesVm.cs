using Catalog.Application.Categorys.Queries.GetCategories;

namespace Catalog.Application.TodoLists.Queries.GetCategories;

public class CategoriesVm
{
    public IList<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
}
