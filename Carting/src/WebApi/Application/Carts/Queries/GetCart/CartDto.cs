using Carting.WebApi.Application.Common.Mappings;
using Carting.WebApi.Domain.Entities;

namespace Carting.WebApi.Application.Carts.Queries.GetCart;

public class CartDto : IMapFrom<Cart>
{
    public CartDto() => Items = new List<CartItemDto>();

    public string Id { get; set; }
    public IList<CartItemDto> Items { get; set; }
}
