namespace Carting.Domain.Exceptions;

public class UnsupportedCurrencyException : Exception
{
    public UnsupportedCurrencyException(string code)
        : base($"Currency \"{code}\" is unsupported.")
    {
    }
}
