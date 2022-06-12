using System.Text.Json.Serialization;
using Catalog.Application.Common.Models;

namespace Catalog.WebApi.Model;

public class HateoasResponse<T>
{
    [JsonPropertyName("_links")]
    public IDictionary<string, LinkDto> Links { get; set; }

    [JsonPropertyName("_embedded")]
    public T Embedded { get; set; }
}
