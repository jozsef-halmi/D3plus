using AutoMapper;
using Carting.WebApi.Application.Common.Mappings;
using Carting.WebApi.Domain.Entities;

namespace Carting.WebApi.Application.Carts.Queries.GetCart;

public class CartItemDto : IMapFrom<CartItem>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CurrencyCode { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? ImageUri { get; set; }
    public string? ImageAltText { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CartItem, CartItemDto>()
            .ForMember(d => d.ImageAltText, opt => opt.MapFrom(s => s.WebImage != null ? s.WebImage.AltText : null))
            .ForMember(d => d.ImageUri, opt => opt.MapFrom(s => s.WebImage != null ? s.WebImage.Uri : null))
            .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(s => s.Currency.ToString()));
    }
}
