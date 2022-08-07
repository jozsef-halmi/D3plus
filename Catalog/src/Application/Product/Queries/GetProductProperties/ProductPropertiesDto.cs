using AutoMapper;
using Catalog.Application.Common.Mappings;
using Catalog.Application.Common.Models;

namespace Catalog.Application.Product.Queries.Common;

public class ProductPropertiesDto : IMapFrom<Domain.Entities.Product>
{
    public int Id { get; set; }

    public IDictionary<string,string> Properties { get; set; }

     public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Product, ProductPropertiesDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(f => f.Id))
            .ForMember(d => d.Properties, opt => opt.Ignore());
    }
}
