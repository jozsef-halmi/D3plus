using Catalog.Application.Categorys.Commands.CreateCategory;
using Catalog.Application.Categorys.Commands.DeleteCategory;
using Catalog.Application.Categorys.Commands.UpdateCategory;
using Catalog.Application.TodoLists.Queries.GetCategories;
using Catalog.GraphQL.GraphQL.Types;
using MediatR;

namespace Catalog.GraphQL.GraphQL;

public class CategoriesDataLoader
{
    private readonly ISender _mediator;

    public CategoriesDataLoader(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        var categories = await _mediator.Send(new GetCategoriesQuery());
        return categories.Categories.Select(c => new Category()
        {
            Id = c.Id,
            ImageUrl = c.ImageUrl,
            Name = c.Name,
            ParentCategoryId = c.ParentCategoryId,
            ParentCategoryName = c.ParentCategoryName
        });
    }


    public async Task<Category> AddCategory(Category category)
    {
        var id = await _mediator.Send(new CreateCategoryCommand()
        {
            Name = category.Name,
            ImageUrl = category.ImageUrl,
            ParentCategoryId = category.ParentCategoryId
        });

        category.Id = id;
        return category;
    }

    public async Task<Category> UpdateCategory(Category category)
    {
        await _mediator.Send(new UpdateCategoryCommand()
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl,
            ParentCategoryId = category.ParentCategoryId
        });

        return category;
    }

    public async Task<Category> DeleteCategory(Category category)
    {
        await _mediator.Send(new DeleteCategoryCommand()
        {
            Id = category.Id
        });

        return category;
    }
}
