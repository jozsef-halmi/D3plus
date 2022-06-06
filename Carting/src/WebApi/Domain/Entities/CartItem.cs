using Carting.WebApi.Domain.Enums;

namespace Carting.WebApi.Domain.Entities;

public class CartItem
{
    // Points to an Id in an external system
    public int Id { get; set; }
    public string Name { get; set; }
    public WebImage? WebImage { get; set; }
    public Currency Currency { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
