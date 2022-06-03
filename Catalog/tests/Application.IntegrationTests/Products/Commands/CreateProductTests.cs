using Catalog.Application.Common.Exceptions;
using Catalog.Application.Products.Commands.CreateProduct;
using Catalog.Application.Products.Exceptions;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Catalog.Application.IntegrationTests.Products.Commands;

using static Testing;

public class CreateProductTests : BaseTestFixture
{

    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateProductCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateProduct()
    {
        var userId = await RunAsDefaultUserAsync();

        var category = await CreateCategory();
        var command = new CreateProductCommand
        {
            Name = "New product",
            Price = 5.99M,
            Amount = 2,
            CategoryId = category.Id
        };

        var productId = await SendAsync(command);

        var product = await FindAsync<Product>(productId);

        product.Should().NotBeNull();
        product.Name.Should().Be(command.Name);
        product.Price.Should().Be(command.Price);
        product.Amount.Should().Be(command.Amount);
        product.CategoryId.Should().Be(command.CategoryId);

        product.CreatedBy.Should().Be(userId);
        product.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        product.LastModifiedBy.Should().Be(userId);
        product.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }

    [Test]
    public async Task InvalidCategoryShouldThrowError()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateProductCommand
        {
            Name = "New product",
            Price = 5.99M,
            Amount = 2,
            CategoryId = -1
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<CategoryNotFoundException>();
    }

    [Test]
    public async Task InvalidAmountShouldThrowError()
    {
        var userId = await RunAsDefaultUserAsync();

        var category = await CreateCategory();

        var command = new CreateProductCommand
        {
            Name = "New product",
            Price = 5.99M,
            Amount = -2,
            CategoryId = category.Id
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task InvalidPriceShouldThrowError()
    {
        var userId = await RunAsDefaultUserAsync();

        var category = await CreateCategory();

        var command = new CreateProductCommand
        {
            Name = "New product",
            Price = -2M,
            Amount = 2,
            CategoryId = category.Id
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task NameTooLongShouldThrowError()
    {
        var userId = await RunAsDefaultUserAsync();

        var category = await CreateCategory();

        var command = new CreateProductCommand
        {
            Name = string.Join("", Enumerable.Repeat("c", 51)),
            Price = 2M,
            Amount = 2,
            CategoryId = category.Id
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task DescriptionAllowsHtml()
    {
        var userId = await RunAsDefaultUserAsync();

        var category = await CreateCategory();

        var command = new CreateProductCommand
        {
            Name = "New product",
            Description = "<html><h1>Description</h1></html>",
            Price = 2M,
            Amount = 2,
            CategoryId = category.Id
        };

        var productId = await SendAsync(command);
        var product = await FindAsync<Product>(productId);

        product.Should().NotBeNull();
        product.Description.Should().Be(command.Description);
        product.CreatedBy.Should().Be(userId);
        product.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        product.LastModifiedBy.Should().Be(userId);
        product.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }

    private async Task<Category> CreateCategory()
    {
        return await AddAsync(new Category
        {
            Name = "Example Category1",
        });
    }
}
