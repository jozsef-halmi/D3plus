using Catalog.Application.Common.Models;
using Catalog.Application.Product.Queries.Common;
using Catalog.Application.Product.Queries.GetProduct;
using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Application.Products.Commands.DeleteProduct;
using Catalog.Application.Products.Commands.UpdateProduct;
using Catalog.Application.TodoLists.Queries.GetProducts;
using Catalog.Application.TodoLists.Queries.GetProductsWithPagination;
using Catalog.WebApi.Helpers;
using Catalog.WebApi.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

public class ProductsController : ApiControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    public ProductsController(LinkGenerator linkGenerator, ISender mediator) : base(mediator)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    public async Task<ActionResult<HateoasResponse<ProductDto>>> GetProduct([FromQuery] GetProductQuery query)
    {
        var product = await _mediator.Send(query);
        return HateoasHelper.CreateLinksForProduct(HttpContext, _linkGenerator, product);
    }

    [HttpGet("Properties")]
    public async Task<ActionResult<ProductPropertiesDto>> GetProductProperties([FromQuery] GetProductPropertiesQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpGet("WithPagination")]
    public async Task<ActionResult<HateoasResponse<ProductsWithPaginationVm>>> GetProductsWithPagination([FromQuery] GetProductsWithPaginationQuery query)
    {
        return HateoasHelper.CreateLinksForProducts(HttpContext, _linkGenerator, await _mediator.Send(query), query.CategoryId);
    }

    [HttpPost]
    [Authorize(Roles = "Manager", Policy = "CatalogApiScope")]
    public async Task<ActionResult<int>> Create(CreateProductCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Manager", Policy = "CatalogApiScope")]
    public async Task<ActionResult> Update(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager", Policy = "CatalogApiScope")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteProductCommand() { Id = id });

        return NoContent();
    }
}