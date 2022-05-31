namespace Carting.Domain.Entities;

public class Cart
{
    public Cart() => Items = new List<CartItem>();

    public string Id { get; set; }
    public IList<CartItem> Items { get; set; }
}
