using Catalog.Application.Common.Models;
using Catalog.Application.TodoLists.Queries.GetCategories;
using Catalog.Application.TodoLists.Queries.GetProducts;
using Catalog.Application.TodoLists.Queries.GetProductsWithPagination;
using Catalog.WebApi.Controllers;
using Catalog.WebApi.Model;

namespace Catalog.WebApi.Helpers;

public static class HateoasHelper
{
    public static HateoasResponse<CategoriesVm> CreateLinksForCategories(HttpContext context, LinkGenerator linkGenerator, CategoriesVm vm)
    {
        foreach (var category in vm.Categories)
        {
            var id = category.Id;

            // Create
            category.Links.Add(HttpMethod.Post.ToString().ToLower(),
                new Application.Common.Models.LinkDto(linkGenerator.GetPathByAction(context, nameof(CategoriesController.Create)),
                    HttpMethod.Post.ToString()));

            // Update
            category.Links.Add(HttpMethod.Put.ToString().ToLower(),
                new Application.Common.Models.LinkDto(linkGenerator.GetPathByAction(context, nameof(CategoriesController.Update), values: new { id }),
                    HttpMethod.Put.ToString()));

            // Delete
            category.Links.Add(HttpMethod.Delete.ToString().ToLower(),
                new Application.Common.Models.LinkDto(linkGenerator.GetPathByAction(context, nameof(CategoriesController.Delete), values: new { id }),
                   
                    HttpMethod.Delete.ToString()));

            // Products
            category.Links.Add("products",
                new Application.Common.Models.LinkDto(linkGenerator.GetPathByAction(context, nameof(ProductsController.GetProductsWithPagination), "products", values: new GetProductsWithPaginationQuery { CategoryId = id }),
                    HttpMethod.Get.ToString()));
        }

        var links = new Dictionary<string, LinkDto>();

        // Self
        links.Add("self",
            new Application.Common.Models.LinkDto(linkGenerator.GetPathByAction(context, nameof(CategoriesController.GetCategories)),
                    HttpMethod.Get.ToString()));

        return new HateoasResponse<CategoriesVm>()
        {
            Embedded = vm,
            Links = links
        };
    }

    public static HateoasResponse<ProductsWithPaginationVm> CreateLinksForProducts(HttpContext context, LinkGenerator linkGenerator, ProductsWithPaginationVm vm, int categoryId)
    {
        foreach (var product in vm.Products.Items)
        {
            var id = product.Id;

            // Create
            product.Links.Add(HttpMethod.Post.ToString().ToLower(),
                new Application.Common.Models.LinkDto(linkGenerator.GetPathByAction(context, nameof(ProductsController.Create)),
                    HttpMethod.Post.ToString()));

            // Update
            product.Links.Add(HttpMethod.Put.ToString().ToLower(),
                new Application.Common.Models.LinkDto(linkGenerator.GetPathByAction(context, nameof(ProductsController.Update), values: new { id }),
                    HttpMethod.Put.ToString()));

            // Delete
            product.Links.Add(HttpMethod.Delete.ToString().ToLower(),
                new Application.Common.Models.LinkDto(linkGenerator.GetPathByAction(context, nameof(ProductsController.Delete), values: new { id }),
                    HttpMethod.Delete.ToString()));
        }

        var links = new Dictionary<string, LinkDto>();
        links.Add("self", new LinkDto(linkGenerator.GetPathByAction(context, nameof(ProductsController.GetProductsWithPagination), "products", values: new GetProductsWithPaginationQuery { CategoryId = categoryId, PageNumber = vm.Products.PageNumber, PageSize = vm.Products.PageSize }),
                    HttpMethod.Get.ToString()));

        if (vm.Products.HasNextPage)
            links.Add("next", new LinkDto(linkGenerator.GetPathByAction(context, nameof(ProductsController.GetProductsWithPagination), "products", values: new GetProductsWithPaginationQuery { CategoryId = categoryId, PageNumber = vm.Products.PageNumber+1, PageSize = vm.Products.PageSize }),
                    HttpMethod.Get.ToString()));
        
        if (vm.Products.HasPreviousPage)
            links.Add("previous", new LinkDto(linkGenerator.GetPathByAction(context, nameof(ProductsController.GetProductsWithPagination), "products", values: new GetProductsWithPaginationQuery { CategoryId = categoryId, PageNumber = vm.Products.PageNumber - 1, PageSize = vm.Products.PageSize }),
                    HttpMethod.Get.ToString()));

        if (vm.Products.TotalPages != vm.Products.PageNumber)
            links.Add("last", new LinkDto(linkGenerator.GetPathByAction(context, nameof(ProductsController.GetProductsWithPagination), "products", values: new GetProductsWithPaginationQuery { CategoryId = categoryId, PageNumber = vm.Products.TotalPages, PageSize = vm.Products.PageSize }),
                    HttpMethod.Get.ToString()));

        return new HateoasResponse<ProductsWithPaginationVm>()
        {
            Embedded = vm,
            Links = links
        };
    }
}
