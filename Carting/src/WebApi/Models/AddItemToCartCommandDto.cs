using Carting.WebApi.Domain.Entities;

namespace Carting.WebApi.Models;

public class AddItemToCartCommandDto
{
    /// <summary>
    /// Item id. Points to an Id in an external system
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Well defined name of the item
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Web image (optional)
    /// </summary>
    public WebImage? WebImage { get; init; }

    /// <summary>
    /// 3-letter currency code
    /// </summary>
    public string CurrencyCode { get; init; }

    /// <summary>
    /// Price, decimal
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Quantity, integer
    /// </summary>
    public int Quantity { get; init; }
}
