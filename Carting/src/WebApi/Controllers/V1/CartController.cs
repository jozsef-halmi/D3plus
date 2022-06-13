using Carting.WebApi.Application.Carts.Queries.GetCart;
using Microsoft.AspNetCore.Mvc;

namespace Carting.WebApi.Controllers.V1;

[ApiVersion("1")]
public class CartsController : ApiControllerBase
{
    /// <summary>
    /// Returns a specific cart and its associated items
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /
    ///     {
    ///        "CartId": 1
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns the cart</response>
    /// <response code="404">If the cart does not exist</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart([FromQuery] GetCartQuery query)
    {
        return await Mediator.Send(query);
    }

    //[HttpPost]
    //public async Task<ActionResult<int>> Create(CreateProductCommand command)
    //{
    //    return await Mediator.Send(command);
    //}

    //[HttpPut("{id}")]
    //public async Task<ActionResult> Update(int id, UpdateProductCommand command)
    //{
    //    if (id != command.Id)
    //    {
    //        return BadRequest();
    //    }

    //    await Mediator.Send(command);

    //    return NoContent();
    //}

    //[HttpDelete("{id}")]
    //public async Task<ActionResult> Delete(int id)
    //{
    //    await Mediator.Send(new DeleteProductCommand() { Id = id });

    //    return NoContent();
    //}
}