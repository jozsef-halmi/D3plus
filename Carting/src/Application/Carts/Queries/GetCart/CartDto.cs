using Carting.Application.Common.Mappings;
using Carting.Domain.Entities;

namespace Carting.Application.Carts.Queries.GetCart;

public class CartDto : IMapFrom<Cart>
{
    public CartDto() => Items = new List<CartItemDto>();

    public string Id { get; set; }
    public IList<CartItemDto> Items { get; set; }
}
