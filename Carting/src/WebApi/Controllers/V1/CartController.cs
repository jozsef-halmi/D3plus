using Carting.WebApi.Application.Carts.Commands.AddItemToCart;
using Carting.WebApi.Application.Carts.Commands.RemoveItemFromCartCommand;
using Carting.WebApi.Application.Carts.Queries.GetCart;
using Carting.WebApi.Models;
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
    /// <response code="500">Server error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart([FromQuery] GetCartQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Adds an item to a specific cart. If there was no cart for the specifiedkey, it creates the cart as well.
    /// </summary>
    /// <param name="cartId">External id of the cart</param>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /some-id/Items
    ///     {
    ///        "Id": 1,
    ///        "Name": "Some item name",
    ///        "CurrencyCode": "EUR",
    ///        "Price":5.99,
    ///        "Quantity":2,
    ///        "WebImage": {
    ///           "Uri": "http://localhost/some-image-url.png",
    ///           "AltText": "Missing image"
    ///         }
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Item created</response>
    /// <response code="422">Request couldn't be processed - item is already added</response>
    /// <response code="500">Server error</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("{cartId}/Items")]
    [HttpPost]
    public async Task<ActionResult> AddItemToCart(string cartId, AddItemToCartCommandDto command)
    {
        var addItemToCartCommand = new AddItemToCartCommand()
        {
            CartId = cartId,
            Id = command.Id,
            CurrencyCode = command.CurrencyCode,
            Name = command.Name,
            Price = command.Price,
            Quantity = command.Quantity,
            WebImage = command.WebImage
        };

        await Mediator.Send(addItemToCartCommand);

        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Adds an item to a specific cart. If there was no cart for the specifiedkey, it creates the cart as well.
    /// </summary>
    /// <param name="cartId">External id of the cart</param>
    /// <param name="itemId">External id of the item</param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /some-id/Items/1
    ///     {
    ///        "Name": "Some item name",
    ///        "CurrencyCode": "EUR",
    ///        "Price":5.99,
    ///        "Quantity":2,
    ///        "WebImage": {
    ///           "Uri": "http://localhost/some-image-url.png",
    ///           "AltText": "Missing image"
    ///         }
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Item deleted</response>
    /// <response code="404">Item is not found</response>
    /// <response code="500">Server error</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("{cartId}/Items/{itemId}")]
    [HttpDelete]
    public async Task<ActionResult> RemoveItemFromCart(string cartId, int itemId)
    {
        await Mediator.Send(new RemoveItemFromCartCommand()
        {
            Id = itemId,
            CartId = cartId
        });

        return Ok();
    }
}