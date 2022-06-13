namespace Catalog.Application.Products.Exceptions;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException()
        : base()
    {
    }

    public CategoryNotFoundException(string message)
        : base(message)
    {
    }

    public CategoryNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CategoryNotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}
