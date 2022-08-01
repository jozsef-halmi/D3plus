using Catalog.Application.TodoLists.Queries.GetCategories;
using Catalog.GraphQL.GraphQL.Types;
using MediatR;
using AutoMapper;
using Catalog.Application.Categorys.Queries.GetCategories;
using Catalog.Application.Categorys.Commands.CreateCategory;
using Catalog.Application.Categorys.Commands.UpdateCategory;

namespace Catalog.GraphQL.GraphQL;

public class CategoriesData
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public CategoriesData(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
        var id = await _mediator.Send(new UpdateCategoryCommand()
        {
            Id = category.Id,
            Name = category.Name,
            ImageUrl = category.ImageUrl,
            ParentCategoryId = category.ParentCategoryId
        });

        return category;
    }
}
