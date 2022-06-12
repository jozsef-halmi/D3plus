namespace Catalog.Application.Common.Models;

public class LinkDto
{
    public string Href { get; private set; }

    public string Method { get; private set; }

    public LinkDto(string href, string method)
    {
        Href = href;
        Method = method;
    }
}