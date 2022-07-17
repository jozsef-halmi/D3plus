using NUnit.Framework;
using FluentAssertions;
using Carting.WebApi.Application.Common.Exceptions;
using Carting.WebApi.Application.Carts.Commands.Update;
using Carting.WebApi.Domain.Entities;
using Carting.WebApi.Domain.Exceptions;

namespace Carting.Application.IntegrationTests.Cart.Commands;

using static Testing;

public class UpdateCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldUpdateCart()
    {
        var cart = new Carting.WebApi.Domain.Entities.Cart()
        {
            Id = $"external-id-{Guid.NewGuid()}",
            Items = new List<CartItem>()
            {
                new CartItem()
                {
                    Id = 999,
                    Name = "ExampleProduct",
                    Price = 5,
                    Currency = WebApi.Domain.Enums.Currency.EUR,
                    Quantity = 1,
                    WebImage = null
                },
                 new CartItem()
                {
                    Id = 998,
                    Name = "ExampleProduct",
                    Price = 5,
                    Currency = WebApi.Domain.Enums.Currency.EUR,
                    Quantity = 1,
                    WebImage = null
                }
            }
        };

        cart = Add<Carting.WebApi.Domain.Entities.Cart>(cart);

        var command = new UpdateItemCommand
        {
            CartId = cart.Id,
            Id = 998,
            Name = "ExampleProduct modified",
            Price = 1,
            CurrencyCode = "EUR",
            Quantity = 2,
            WebImage = new WebImage()
            {
                AltText = "AltText",
                Uri = new Uri("http://localhost/example.jpg")
            }
        };

        var id = await SendAsync(command);

        var updatedCart = Find<WebApi.Domain.Entities.Cart>(id);

        updatedCart.Should().NotBeNull();
        cart.Items.Should().NotBeNull();
        cart.Items.Should().HaveCount(2);
        var updatedCartItem = updatedCart.Items.FirstOrDefault(item => item.Id == command.Id);

        updatedCartItem.Should().NotBeNull();
        updatedCartItem.Name.Should().Be(command.Name);
        updatedCartItem.Price.Should().Be(command.Price);
        updatedCartItem.Quantity.Should().Be(command.Quantity);
        updatedCartItem.WebImage.Uri.Should().Be(command.WebImage.Uri);
        updatedCartItem.WebImage.AltText.Should().Be(command.WebImage.AltText);
    }

    [Test]
    public async Task InvalidPriceShouldThrowError()
    {
        var cart = new Carting.WebApi.Domain.Entities.Cart()
        {
            Id = $"external-id-{Guid.NewGuid()}",
            Items = new List<CartItem>()
            {
                 new CartItem()
                {
                    Id = 998,
                    Name = "ExampleProduct",
                    Price = 5,
                    Currency = WebApi.Domain.Enums.Currency.EUR,
                    Quantity = 1,
                    WebImage = null
                }
            }
        };

        cart = Add<Carting.WebApi.Domain.Entities.Cart>(cart);

        var command = new UpdateItemCommand
        {
            CartId = cart.Id,
            Id = 998,
            Name = "ExampleProduct modified",
            Price = -1,
            CurrencyCode = "EUR",
            Quantity = 2,
            WebImage = new WebImage()
            {
                AltText = "AltText",
                Uri = new Uri("http://localhost/example.jpg")
            }
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }


    [Test]
    public async Task InvalidCurrencyShouldThrowError()
    {
        var cart = new Carting.WebApi.Domain.Entities.Cart()
        {
            Id = $"external-id-{Guid.NewGuid()}",
            Items = new List<CartItem>()
            {
                 new CartItem()
                {
                    Id = 998,
                    Name = "ExampleProduct",
                    Price = 5,
                    Currency = WebApi.Domain.Enums.Currency.EUR,
                    Quantity = 1,
                    WebImage = null
                }
            }
        };

        cart = Add<Carting.WebApi.Domain.Entities.Cart>(cart);

        var command = new UpdateItemCommand
        {
            CartId = cart.Id,
            Id = 998,
            Name = "ExampleProduct modified",
            Price = -1,
            CurrencyCode = "GBP",
            Quantity = 2,
            WebImage = new WebImage()
            {
                AltText = "AltText",
                Uri = new Uri("http://localhost/example.jpg")
            }
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task TooLongNameShouldThrowError()
    {
        var cart = new Carting.WebApi.Domain.Entities.Cart()
        {
            Id = $"external-id-{Guid.NewGuid()}",
            Items = new List<CartItem>()
            {
                 new CartItem()
                {
                    Id = 998,
                    Name = "ExampleProduct",
                    Price = 5,
                    Currency = WebApi.Domain.Enums.Currency.EUR,
                    Quantity = 1,
                    WebImage = null
                }
            }
        };

        cart = Add<Carting.WebApi.Domain.Entities.Cart>(cart);

        var command = new UpdateItemCommand
        {
            CartId = cart.Id,
            Id = 998,
            Name = string.Join("", Enumerable.Repeat("c", 51)),
            Price = -1,
            CurrencyCode = "EUR",
            Quantity = 2,
            WebImage = new WebImage()
            {
                AltText = "AltText",
                Uri = new Uri("http://localhost/example.jpg")
            }
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }
}
