using Carting.WebApi.Application.Carts.Commands.AddItemToCart;
using Carting.WebApi.Application.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Carting.Application.IntegrationTests.Cart.Commands;

using static Testing;

public class AddItemToCartCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateCart()
    {
        var command = new AddItemToCartCommand($"external-id-{Guid.NewGuid()}", 
            1, "ExampleProduct", null, "EUR", 5, 1);

        var id = await SendAsync(command);

        var cart = Find<WebApi.Domain.Entities.Cart>(id);

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
        var command = new AddItemToCartCommand($"external-id-{Guid.NewGuid()}",
         1, "ExampleProduct", null, "EUR", -1, 1);
   
        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldAddItemToExistingCart()
    {
        var cartId = $"external-id-{Guid.NewGuid()}";
        var command1 = new AddItemToCartCommand(cartId,
         1, "ExampleProduct", null, "EUR", 5, 1);

        var command2 = new AddItemToCartCommand(cartId,
         2, "ExampleProduct2", null, "EUR", 6, 2);

        var cartId1 = await SendAsync(command1);
        var cartId2 = await SendAsync(command2);

        cartId1.Should().Be(cartId2);

        var cart = Find<WebApi.Domain.Entities.Cart>(cartId1);
        cart.Items.Should().HaveCount(2);
    }

    [Test]
    public async Task DuplicateCartItemShouldThrowError()
    {
        var cartId = $"external-id-{Guid.NewGuid()}";
        var command1 = new AddItemToCartCommand(cartId,
         2, "ExampleProduct", null, "EUR", 5, 1);

        var command2 = new AddItemToCartCommand(cartId,
         2, "ExampleProduct2", null, "EUR", 5, 2);

        var id1 = await SendAsync(command1);

        Func<Task> addItemToCartAgain = async () => await SendAsync(command2);
        await addItemToCartAgain.Should().ThrowAsync<ItemAlreadyAddedToCartException>();
    }

    [Test]
    public async Task TooLongNameShouldThrowError()
    {
        var command = new AddItemToCartCommand($"external-id-{Guid.NewGuid()}",
         1, string.Join("", Enumerable.Repeat("c", 51)), null, "EUR", 5, 1);

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }
}
