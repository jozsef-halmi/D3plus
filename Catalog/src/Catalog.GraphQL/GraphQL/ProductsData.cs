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


    public async Task<Category> GetCategory(Product product)
    {
        var categoryDto = await _mediator.Send(new GetCategoryQuery() { Id = product.CategoryId });
        return new Category()
        {
            Id = categoryDto.Id,
            ImageUrl = categoryDto.ImageUrl,
            Name = categoryDto.Name,
            ParentCategoryId = categoryDto.ParentCategoryId,
            ParentCategoryName = categoryDto.ParentCategoryName
        };
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
