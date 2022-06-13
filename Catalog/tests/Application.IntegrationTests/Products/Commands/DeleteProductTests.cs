using Catalog.Application.Common.Exceptions;
using Catalog.Application.Products.Commands.DeleteProduct;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Catalog.Application.Products.Commands.CreateProduct;

namespace Catalog.Application.IntegrationTests.Products.Commands;

using static Testing;

public class DeleteProductTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteProduct()
    {
        var category = await AddAsync(new Category
        {
            Name = "Example Category1",
        });

        var product = await AddAsync(new Product()
        {
            Name = "New product",
            Price = 5.99M,
            Amount = 2,
            CategoryId = category.Id
        });

        var deleteCommand = new DeleteProductCommand
        {
            ProductId = product.Id
        };

        var deletedProductId = await SendAsync(deleteCommand);

        var removedProduct = await FindAsync<Product>(product.Id);

        removedProduct.Should().BeNull();

        (await FindAsync<Product>(deletedProductId)).Should().BeNull();

    }

    [Test]
    public async Task ShouldThrowNotFound()
    {
        var command = new DeleteProductCommand
        {
            ProductId = 9999
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
