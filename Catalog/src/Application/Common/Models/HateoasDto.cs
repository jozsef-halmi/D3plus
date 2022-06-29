using System.Text.Json.Serialization;

namespace Catalog.Application.Common.Models;

public class HateoasDto
{
    public HateoasDto()
    {
        Links = new Dictionary<string, LinkDto>();
    }

    [JsonPropertyName("_links")]
    public IDictionary<string, LinkDto> Links { get; set; }
}
