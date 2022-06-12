using Catalog.Application.Common.Models;
using Catalog.Application.Product.Queries.GetProductsWithPagination;
using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Application.Products.Commands.DeleteProduct;
using Catalog.Application.Products.Commands.UpdateProduct;
using Catalog.Application.TodoLists.Queries.GetProducts;
using Catalog.Application.TodoLists.Queries.GetProductsWithPagination;
using Catalog.WebApi.Helpers;
using Catalog.WebApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

public class ProductsController : ApiControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    public ProductsController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    public async Task<ActionResult<HateoasResponse<ProductsWithPaginationVm>>> GetProductsWithPagination([FromQuery] GetProductsWithPaginationQuery query)
    {
        return HateoasHelper.CreateLinksForProducts(HttpContext, _linkGenerator, await Mediator.Send(query), query.CategoryId);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteProductCommand() {  Id = id });

        return NoContent();
    }
}