using AutoMapper;
using Catalog.Application.Common.Mappings;
using Catalog.Application.Common.Models;

namespace Catalog.Application.Product.Queries.Common;

public class ProductDto : HateoasDto, IMapFrom<Domain.Entities.Product>
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
        profile.CreateMap<Domain.Entities.Product, ProductDto>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
            .ForMember(d => d.Links, opt => opt.Ignore());
    }
}
