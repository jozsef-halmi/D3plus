namespace Carting.WebApi.Domain.Entities;

public class WebImage
{
    /// <summary>
    /// Uri of the image
    /// </summary>
    public Uri? Uri { get; set; }

    /// <summary>
    /// Alt text, it will be shown in case the client fails to load the image
    /// </summary>
    public string? AltText { get; set; }
}
