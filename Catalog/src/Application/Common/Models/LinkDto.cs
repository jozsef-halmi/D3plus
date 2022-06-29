namespace Catalog.Application.Common.Models;

public class LinkDto
{
    public string Href { get; init; }

    public string Method { get; init; }

    public LinkDto(string href, string method)
    {
        Href = href;
        Method = method;
    }
}