﻿using Carting.WebApi.Application.Carts.Commands.AddItemToCart;
using Carting.WebApi.Application.Carts.Commands.RemoveItemFromCartCommand;
using Carting.WebApi.Application.Carts.Queries.GetCart;
using Carting.WebApi.Application.Common.Exceptions;
using Carting.WebApi.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Carting.Application.IntegrationTests.Cart.Commands;

using static Testing;

public class RemoveItemFromCartCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRemoveCartItemFromCart()
    {
        // Arrange
        var cartId = $"external-id-{Guid.NewGuid()}";
        var cartItemId = 11;
    
        Add(new Carting.WebApi.Domain.Entities.Cart()
        {
            Id = cartId,
            Items = new List<Carting.WebApi.Domain.Entities.CartItem>()
            {
                new Carting.WebApi.Domain.Entities.CartItem()
                {
                    Name = "ExampleProduct",
                    Price = 5,
                    Currency = WebApi.Domain.Enums.Currency.EUR,
                    Quantity = 1,
                    WebImage = null,
                    Id = cartItemId
                }
            }
        });

        var removeItemFromCartCommand = new RemoveItemFromCartCommand()
        {
            CartId = cartId,
            Id = cartItemId,
        };

        // Act
        await SendAsync(removeItemFromCartCommand);


        // Assert
        var query = new GetCartQuery() { CartId = cartId };
        var cart = await SendAsync(query);

        cart.Should().NotBeNull();
        cart.Id.Should().Be(cartId);
        cart.Items.Should().NotBeNull();
        cart.Items.Should().HaveCount(0);
    }

    [Test]
    public async Task InvalidCartIdShouldReturnNotFound()
    {
        var command = new RemoveItemFromCartCommand
        {
            CartId = $"external-id-{Guid.NewGuid()}",
            Id = 1
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<NotFoundException>();
    }


    [Test]
    public async Task InvalidCartItemIdShouldReturnNotFound()
    {
        // Arrange
        var cartId = $"external-id-{Guid.NewGuid()}";
        var cartItemId = 11;

        Add(new Carting.WebApi.Domain.Entities.Cart()
        {
            Id = cartId,
            Items = new List<Carting.WebApi.Domain.Entities.CartItem>()
            {
                new Carting.WebApi.Domain.Entities.CartItem()
                {
                    Name = "ExampleProduct",
                    Price = 5,
                    Currency = Currency.EUR,
                    Quantity = 1,
                    WebImage = null,
                }
            }
        });

        var removeItemFromCartCommand = new RemoveItemFromCartCommand()
        {
            CartId = cartId,
            Id = cartItemId+1,
        };

        // Act and assert
        Func<Task> act = async () => await SendAsync(removeItemFromCartCommand);
        await act.Should().ThrowAsync<NotFoundException>();
    }

}
