using Catalog.Application.Common.Models;
using Catalog.Application.Product.Queries.GetProductsWithPagination;
using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Application.Products.Commands.DeleteProduct;
using Catalog.Application.Products.Commands.UpdateProduct;
using Catalog.Application.TodoLists.Queries.GetProductsWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

public class ProductsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductDto>>> GetProductsWithPagination([FromQuery] GetProductsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
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