using Catalog.Application.Common.Exceptions;
using Catalog.Application.Products.Commands.UpdateProduct;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Catalog.Application.IntegrationTests.Products.Commands;

using static Testing;

public class UpdateProductTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new UpdateProductCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldUpdateProduct()
    {
        var category1 = await AddAsync(new Category
        {
            Name = "Example Category1",
        });

        var category2 = await AddAsync(new Category
        {
            Name = "Example Category2",
        });


        var product = await AddAsync(new Domain.Entities.Product()
        {
            Name = "New product",
            Price = 5.99M,
            Amount = 2,
            CategoryId = category1.Id
        });

        var updateCommand = new UpdateProductCommand()
        {
            Id = product.Id,
            Name = "New Product modified",
            ImageUrl = new Uri("http://localhost/example-modified.jpg"),
            CategoryId = category2.Id,
            Amount = 3,
            Price = 7.99M,
            Description = "Updated description"
        };

        var updatedProductId = await SendAsync(updateCommand);
        var updatedProduct = await FindAsync<Domain.Entities.Product>(updatedProductId);


        product.Should().NotBeNull();
        updatedProduct.Should().NotBeNull();
        updatedProductId.Should().Be(product.Id);


        updatedProduct.Name.Should().Be(updateCommand.Name);
        updatedProduct.ImageUrl.Should().Be(updateCommand.ImageUrl);
        updatedProduct.CategoryId.Should().Be(updateCommand.CategoryId);
        updatedProduct.Amount.Should().Be(updateCommand.Amount);
        updatedProduct.Price.Should().Be(updateCommand.Price);
        updatedProduct.Description.Should().Be(updateCommand.Description);

        updatedProduct.CreatedBy.Should().Be(product.CreatedBy);
        updatedProduct.Created.Should().Be(product.Created);

        updatedProduct.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }

    [Test]
    public async Task ShouldThrowNotFound()
    {
        var updateProductCommand = new UpdateProductCommand
        {
            Id = 9999
        };

        Func<Task> act = async () => await SendAsync(updateProductCommand);
        await act.Should().ThrowAsync<ValidationException>();
    }
}
