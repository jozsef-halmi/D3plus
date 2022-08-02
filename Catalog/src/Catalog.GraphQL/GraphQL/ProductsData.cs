using Catalog.Application.TodoLists.Queries.GetCategories;
using Catalog.GraphQL.GraphQL.Types;
using MediatR;
using AutoMapper;
using Catalog.Application.TodoLists.Queries.GetProducts;
using Catalog.Application.TodoLists.Queries.GetCategory;

namespace Catalog.GraphQL.GraphQL;

public class ProductsData
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private IEnumerable<Category> _categoriesCache;

    public ProductsData(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var productsVm = await _mediator.Send(new GetProductsQuery());
        return productsVm.Products.Select(c => new Product()
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
        //if (_categoriesCache == null)
        //{
        //    var categoriesVm = await _mediator.Send(new GetCategoriesQuery() { });

        //    _categoriesCache = categoriesVm.Categories.Select(v => new Category() 
        //    {
        //            Id = v.Id,
        //            ImageUrl = v.ImageUrl,
        //            Name = v.Name,
        //            ParentCategoryId = v.ParentCategoryId,
        //            ParentCategoryName = v.ParentCategoryName
        //        });
        //}

        //return _categoriesCache.FirstOrDefault(c => c.Id == categoryId);

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
