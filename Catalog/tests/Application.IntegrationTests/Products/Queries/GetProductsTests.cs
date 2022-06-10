using Catalog.Application.Common.Exceptions;
using Catalog.Application.TodoLists.Queries.GetProducts;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Catalog.Application.IntegrationTests.TodoLists.Queries;

using static Testing;

public class GetProductsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllProducts()
    {
        var category = await AddAsync(new Category
        {
            Name = "Example Category",
        });

        var productToInsert1 = new Domain.Entities.Product
        {
            Name = "Example Product1",
            CategoryId = category.Id,
            Price = 1,
            Amount = 1
        };
        await AddAsync(productToInsert1);

        var productToInsert2 = new Domain.Entities.Product
        {
            Name = "Example Product2",
            CategoryId = category.Id,
            Price = 1,
            Amount = 1
        };
        await AddAsync(productToInsert2);
        var products = GetAll<Domain.Entities.Product>();

        var query = new GetProductsQuery();

        var result = await SendAsync(query);

        result.Products.Should().HaveCount(products.Count);

        var product1 = result.Products.FirstOrDefault(p => p.Name == productToInsert1.Name);
        product1.Should().NotBeNull();
        product1.Name.Should().Be(productToInsert1.Name);
        product1.Description.Should().Be(productToInsert1.Description);
        product1.ImageUrl.Should().Be(productToInsert1.ImageUrl);
        product1.Amount.Should().Be(productToInsert1.Amount);
        product1.Price.Should().Be(productToInsert1.Price);
        product1.CategoryId.Should().Be(productToInsert1.CategoryId);
        product1.CategoryName.Should().Be(category.Name);

        var product2 = result.Products.FirstOrDefault(p => p.Name == productToInsert2.Name);
        product2.Should().NotBeNull();
        product2.Name.Should().Be(productToInsert2.Name);
        product2.Description.Should().Be(productToInsert2.Description);
        product2.ImageUrl.Should().Be(productToInsert2.ImageUrl);
        product2.Amount.Should().Be(productToInsert2.Amount);
        product2.Price.Should().Be(productToInsert2.Price);
        product2.CategoryId.Should().Be(productToInsert2.CategoryId);
        product2.CategoryName.Should().Be(category.Name);
    }
}
