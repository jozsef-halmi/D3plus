using Catalog.Application.TodoLists.Queries.GetCategories;
using Catalog.GraphQL.GraphQL.Types;
using MediatR;

namespace Catalog.GraphQL.GraphQL;

public class CategoriesData
{
    private readonly List<Category> _categories = new List<Category>();
    private readonly ISender _mediator;

    public CategoriesData(ISender mediator)
    {
        _mediator = mediator;
        _categories = new List<Category>()
        {
            new Category()
            {
                Id = 1,
                Name = "Dummy category"
            },
             new Category()
            {
                Id = 2,
                Name = "Dummy category2"
            }
        };
    }

    public Task<Category> GetCategoryByIdAsync(int id)
    {
        return Task.FromResult(_categories.FirstOrDefault(h => h.Id == id));
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        var categories = await _mediator.Send(new GetCategoriesQuery());
        return categories?.Categories?.Select(c => new Category()
        {
            Id = c.Id,
            Name = c.Name
        });
        //return Task.FromResult(_categories.AsEnumerable());
    }

}
