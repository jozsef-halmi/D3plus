using Carting.Domain.Exceptions;
using Carting.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace Carting.Domain.UnitTests.ValueObjects;

public class CurrencyTests
{
    [Test]
    public void ShouldReturnCorrectCurrencyCode()
    {
        var code = "EUR";

        var currency = Currency.From(code);

        currency.Code.Should().Be(code);
    }

    [Test]
    public void ToStringReturnsCode()
    {
        var currency = Currency.EUR;

        currency.ToString().Should().Be(currency.Code);
    }

    [Test]
    public void ShouldPerformImplicitConversionToCurrencyCodeString()
    {
        string code = Currency.EUR;

        code.Should().Be("EUR");
    }

    [Test]
    public void ShouldPerformExplicitConversionGivenSupportedCurrencyCode()
    {
        var currency = (Currency)"EUR";

        currency.Should().Be(Currency.EUR);
    }

    [Test]
    public void ShouldThrowUnsupportedCurrencyExceptionGivenNotSupportedCurrencyCode()
    {
        FluentActions.Invoking(() => Currency.From("GBP"))
            .Should().Throw<UnsupportedCurrencyException>();
    }
}
