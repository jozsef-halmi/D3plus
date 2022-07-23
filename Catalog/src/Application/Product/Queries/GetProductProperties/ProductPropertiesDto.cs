using AutoMapper;
using Catalog.Application.Common.Mappings;
using Catalog.Application.Common.Models;

namespace Catalog.Application.Product.Queries.Common;

public class ProductPropertiesDto : HateoasDto, IMapFrom<Domain.Entities.Product>
{
    public int Id { get; set; }

    public IDictionary<string,string> Properties { get; set; }
}
