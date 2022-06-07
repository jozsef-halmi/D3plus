using Carting.WebApi.Domain.Enums;
using Carting.WebApi.Domain.Exceptions;

namespace Carting.WebApi.Domain.Extensions;

public static class CurrencyExtensions
{
    public static Currency ToCurrency(this string currencyCode)
    {
        if (!Enum.TryParse<Currency>(currencyCode, out var result))
            throw new UnsupportedCurrencyException(currencyCode);

        return result;
    }
}
