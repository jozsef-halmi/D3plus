using Infrastructure.GraphQL.Types;

namespace Infrastructure.GraphQL;

public class CategoriesData
{
    private readonly List<Category> _categories = new List<Category>();

    public CategoriesData()
    {
        _categories = new List<Category>()
        {
            new Category()
            {
                Id = 1,
                Name = "Dummy category"
            }
        };
    }

    public Task<Category> GetCategoryByIdAsync(int id)
    {
        return Task.FromResult(_categories.FirstOrDefault(h => h.Id == id));
    }

}
