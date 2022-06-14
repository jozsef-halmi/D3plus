using Carting.WebApi.Domain.Entities;

namespace Carting.WebApi.Models;

public class AddItemToCartCommandDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public WebImage? WebImage { get; init; }
    public string CurrencyCode { get; init; }
    public decimal Price { get; init; }
    public int Quantity { get; init; }
}
