using Carting.WebApi.Domain.Enums;
using Carting.WebApi.Domain.Exceptions;
using Carting.WebApi.Domain.Extensions;
using FluentAssertions;
using NUnit.Framework;

public class CurrencyExtensionsTests
{
    [Test]
    public void ShouldReturnCorrectCurrencyCode()
    {
        var code = "EUR";

        var currency = code.ToCurrency();

        currency.Should().Be(Currency.EUR);
    }

    [Test]
    public void ToStringReturnsCode()
    {
        var currency = Currency.EUR;

        currency.ToString().Should().Be("EUR");
    }


    [Test]
    public void ShouldThrowUnsupportedCurrencyExceptionGivenNotSupportedCurrencyCode()
    {
        FluentActions.Invoking(() => "GBP".ToCurrency())
            .Should().Throw<UnsupportedCurrencyException>();
    }
}
