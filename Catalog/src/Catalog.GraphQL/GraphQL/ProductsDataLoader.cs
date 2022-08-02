using Catalog.Application.TodoLists.Queries.GetCategories;
using Catalog.GraphQL.GraphQL.Types;
using MediatR;
using AutoMapper;
using Catalog.Application.TodoLists.Queries.GetProducts;
using Catalog.Application.TodoLists.Queries.GetCategory;
using Catalog.Application.TodoLists.Queries.GetProductsWithPagination;
using Catalog.Application.Products.Commands.UpdateProduct;
using Catalog.Application.Products.Commands.DeleteProduct;
using Catalog.Application.Products.Commands.CreateProduct;

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

    public async Task<Product> AddProduct(Product product)
    {
        var id = await _mediator.Send(new CreateProductCommand()
        {
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            Amount = product.Amount,
            Description = product.Description,
            Price = product.Price
        });

        product.Id = id;
        return product;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        var id = await _mediator.Send(new UpdateProductCommand()
        {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            Amount = product.Amount,
            Description = product.Description,
            Price = product.Price
        });

        return product;
    }

    public async Task<Product> DeleteProduct(Product product)
    {
        var id = await _mediator.Send(new DeleteProductCommand()
        {
            Id = product.Id
        });

        return product;
    }
}
