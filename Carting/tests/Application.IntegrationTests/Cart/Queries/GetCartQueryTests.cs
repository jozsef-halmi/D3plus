using NUnit.Framework;
using FluentAssertions;
using Carting.WebApi.Application.Common.Exceptions;
using Carting.WebApi.Application.Carts.Commands.AddItemToCart;
using Carting.WebApi.Domain.Entities;
using Carting.WebApi.Application.Carts.Queries.GetCart;

namespace Carting.Application.IntegrationTests.Cart.Queries;


using static Testing;

public class GetCartQueryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnCart()
    {
        var cartToInsert = new Carting.WebApi.Domain.Entities.Cart()
        {
            Id = $"external-id-{Guid.NewGuid()}",
            Items = new List<Carting.WebApi.Domain.Entities.CartItem>()
            {
                new Carting.WebApi.Domain.Entities.CartItem()
                {
                    Id = 1,
                    Name = "ExampleProduct",
                    Price = 5,
                    Currency = WebApi.Domain.Enums.Currency.EUR,
                    Quantity = 1,
                    WebImage = new WebImage() {
                        AltText="ExampleAltText",
                        Uri = new Uri("http://localhost/example-image.jpg")
                    },
                }
            }
        };
        Add(cartToInsert);

        var query = new GetCartQuery() { CartId = cartToInsert.Id };
        var result = await SendAsync(query);

        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items.Should().HaveCount(cartToInsert.Items.Count);
        var cartItem = result.Items.First();
        cartItem.Name.Should().Be(cartToInsert.Items.First().Name);
        cartItem.Price.Should().Be(cartToInsert.Items.First().Price);
        cartItem.CurrencyCode.Should().Be(cartToInsert.Items.First().Currency.ToString());
        cartItem.Quantity.Should().Be(cartToInsert.Items.First().Quantity);
        cartItem.ImageAltText.Should().Be(cartToInsert.Items.First().WebImage.AltText);
        cartItem.ImageUri.Should().Be(cartToInsert.Items.First().WebImage.Uri.ToString());
    }

    [Test]
    public async Task NonExistentCartShouldReturnNotFound()
    {
        var id = $"external-id-{Guid.NewGuid()}";

        var query = new GetCartQuery() { CartId = id };

        Func<Task> act = async () => await SendAsync(query);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task EmptyCartIdShouldReturnValidationError()
    {
        var query = new GetCartQuery() { CartId = string.Empty };

        Func<Task> act = async () => await SendAsync(query);
        await act.Should().ThrowAsync<ValidationException>();
    }
}
