using NUnit.Framework;
using FluentAssertions;
using Carting.Application.Common.Exceptions;
using Carting.Application.Carts.Commands.AddItemToCart;

namespace Carting.Application.IntegrationTests.Cart.Commands;

using static Testing;

public class AddItemToCartCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateCart()
    {
        var command = new AddItemToCartCommand
        {
            CartId = $"external-id-{Guid.NewGuid()}",
            Id = 1,
            Name = "ExampleProduct",
            Price = 5,
            CurrencyCode = "EUR",
            Quantity = 1,
            WebImage = null
        };

        var id = await SendAsync(command);

        var cart = Find<Carting.Domain.Entities.Cart>(id);

        cart.Should().NotBeNull();
        cart.Id.Should().Be(command.CartId);
        cart.Items.Should().NotBeNull();
        cart.Items.Should().HaveCount(1);

        var cartItem = cart.Items.FirstOrDefault(x => x.Id == command.Id);
        cartItem.Should().NotBeNull();
        cartItem.Name.Should().Be(command.Name);
        cartItem.Price.Should().Be(command.Price);
        cartItem.Quantity.Should().Be(command.Quantity);
    }

    [Test]
    public async Task InvalidPriceShouldThrowError()
    {
        var command = new AddItemToCartCommand
        {
            CartId = $"external-id-{Guid.NewGuid()}",
            Id = 1,
            Name = "ExampleProduct",
            Price = -1,
            CurrencyCode = "EUR",
            Quantity = 1,
            WebImage = null
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldAddItemToExistingCart()
    {
        var cartId = $"external-id-{Guid.NewGuid()}";
        var command1 = new AddItemToCartCommand
        {
            CartId = cartId,
            Id = 1,
            Name = "ExampleProduct",
            Price = 5,
            CurrencyCode = "EUR",
            Quantity = 1,
            WebImage = null
        };

        var command2 = new AddItemToCartCommand
        {
            CartId = cartId,
            Id = 2,
            Name = "ExampleProduct2",
            Price = 6,
            CurrencyCode = "EUR",
            Quantity = 2,
            WebImage = null
        };

        var id1 = await SendAsync(command1);
        var id2 = await SendAsync(command2);

        id1.Should().Be(id2);

        var cart = Find<Carting.Domain.Entities.Cart>(id1);
        cart.Items.Should().HaveCount(2);

    }

    [Test]
    public async Task DuplicateCartItemShouldThrowError()
    {
        var cartId = $"external-id-{Guid.NewGuid()}";
        var command1 = new AddItemToCartCommand
        {
            CartId = cartId,
            Id = 2,
            Name = "ExampleProduct",
            Price = 5,
            CurrencyCode = "EUR",
            Quantity = 1,
            WebImage = null
        };

        var command2 = new AddItemToCartCommand
        {
            CartId = cartId,
            Id = 2,
            Name = "ExampleProduct2",
            Price = 6,
            CurrencyCode = "EUR",
            Quantity = 2,
            WebImage = null
        };

        var id1 = await SendAsync(command1);

        Func<Task> addItemToCartAgain = async () => await SendAsync(command2);
        await addItemToCartAgain.Should().ThrowAsync<ItemAlreadyAddedToCartException>();
    }

    [Test]
    public async Task TooLongNameShouldThrowError()
    {
        var command = new AddItemToCartCommand
        {
            CartId = $"external-id-{Guid.NewGuid()}",
            Id = 1,
            Name = string.Join("",Enumerable.Repeat("c", 201)),
            Price = 5,
            CurrencyCode = "EUR",
            Quantity = 1,
            WebImage = null
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }
}
