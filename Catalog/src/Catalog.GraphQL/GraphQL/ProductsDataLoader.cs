using Catalog.Application.TodoLists.Queries.GetCategories;
using Catalog.GraphQL.GraphQL.Types;
using MediatR;
using AutoMapper;
using Catalog.Application.TodoLists.Queries.GetProducts;
using Catalog.Application.TodoLists.Queries.GetCategory;
using Catalog.Application.TodoLists.Queries.GetProductsWithPagination;

namespace Catalog.GraphQL.GraphQL;

public class ProductsDataLoader
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public ProductsDataLoader(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Product>> GetProducts(int categoryId, int pageNumber, int pageSize)
    {
        var productsVm = await _mediator.Send(new GetProductsWithPaginationQuery() { CategoryId = categoryId, PageNumber = pageNumber, PageSize = pageSize });
        return productsVm.Products.Items.Select(c => new Product()
        {
            Id = c.Id,
            ImageUrl = c.ImageUrl,
            Name = c.Name,
            Amount = c.Amount,
            CategoryId = c.CategoryId,
            CategoryName = c.CategoryName,
            Description = c.Description,
            Price = c.Price
        });
    }


    public async Task<Category> GetCategory(int categoryId)
    {
        var categoryDto = await _mediator.Send(new GetCategoryQuery() { Id = categoryId });
        return new Category()
        {
            Id = categoryDto.Id,
            ImageUrl = categoryDto.ImageUrl,
            Name = categoryDto.Name,
            ParentCategoryId = categoryDto.ParentCategoryId,
            ParentCategoryName = categoryDto.ParentCategoryName
        };
    }

    public async Task<IDictionary<int, Category>> GetCategoriesByIdAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken)
    {
        var categoriesVm = await _mediator.Send(new GetCategoriesQuery() { });

        return categoriesVm.Categories.Where(c => categoryIds.Contains(c.Id))
            .ToDictionary(k => k.Id, v => new Category()
            {
                Id = v.Id,
                ImageUrl = v.ImageUrl,
                Name = v.Name,
                ParentCategoryId = v.ParentCategoryId,
                ParentCategoryName = v.ParentCategoryName
            });
    }

    //public async Task<Category> AddCategory(Category category)
    //{
    //    var id = await _mediator.Send(new CreateCategoryCommand()
    //    {
    //        Name = category.Name,
    //        ImageUrl = category.ImageUrl,
    //        ParentCategoryId = category.ParentCategoryId
    //    });

    //    category.Id = id;
    //    return category;
    //}

    //public async Task<Category> UpdateCategory(Category category)
    //{
    //    var id = await _mediator.Send(new UpdateCategoryCommand()
    //    {
    //        Id = category.Id,
    //        Name = category.Name,
    //        ImageUrl = category.ImageUrl,
    //        ParentCategoryId = category.ParentCategoryId
    //    });

    //    return category;
    //}

    //public async Task<Category> DeleteCategory(Category category)
    //{
    //    var id = await _mediator.Send(new DeleteCategoryCommand()
    //    {
    //        Id = category.Id
    //    });

    //    return category;
    //}
}
