namespace Carting.Domain.ValueObjects;

public class Currency : ValueObject
{
    static Currency()
    {
    }

    private Currency()
    {
    }

    public Currency(string code)
    {
        Code = code;
    }

    public static Currency From(string code)
    {
        var currency = new Currency { Code = code };

        if (!SupportedCurrencys.Contains(currency))
        {
            throw new UnsupportedCurrencyException(code);
        }

        return currency;
    }

    public static Currency EUR => new("EUR");

    public string Code { get; private set; } = "EUR";

    public static implicit operator string(Currency Currency)
    {
        return Currency.ToString();
    }

    public static explicit operator Currency(string code)
    {
        return From(code);
    }

    public override string ToString()
    {
        return Code;
    }

    protected static IEnumerable<Currency> SupportedCurrencys
    {
        get
        {
            yield return EUR;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
