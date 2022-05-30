namespace Carting.Domain.Entities;

public class Cart
{
    public string Id { get; set; }
    public IList<CartItem> Items { get; set; }
}
