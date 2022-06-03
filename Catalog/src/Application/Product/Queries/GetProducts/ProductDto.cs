using AutoMapper;
using Catalog.Application.Common.Mappings;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Queries.GetProducts;

public class ProductDto : IMapFrom<Product>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Uri? ImageUrl { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public decimal Price { get; set; }

    public int Amount { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductDto>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));
    }
}
