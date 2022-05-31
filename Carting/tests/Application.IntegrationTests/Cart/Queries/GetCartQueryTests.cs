using Carting.Application.Carts.Commands.AddItemToCart;
using Carting.Application.Carts.Queries.GetCart;
using NUnit.Framework;
using FluentAssertions;

namespace Carting.Application.IntegrationTests.Cart.Queries;


using static Testing;

public class GetCartQueryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnCart()
    {
        var command = new AddItemToCartCommand
        {
            CartId = $"external-id-{Guid.NewGuid()}",
            Id = 1,
            Name = "ExampleProduct",
            Price = 5,
            CurrencyCode = "EUR",
            Quantity = 1,
            WebImage = new Domain.Entities.WebImage() { 
                AltText="ExampleAltText", 
                Uri = new Uri("http://localhost/example-image.jpg") 
            }
        };

        var id = await SendAsync(command);

        var query = new GetCartQuery() { CartId = id };
        var result = await SendAsync(query);

        result.Should().NotBeNull();

    }

}
