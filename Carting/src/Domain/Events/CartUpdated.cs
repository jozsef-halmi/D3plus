namespace Carting.Domain.Events;

public class CartUpdated : BaseEvent
{
    public CartUpdated(Cart cart)
    {
        Cart = cart;
    }

    public Cart Cart { get; }
}
