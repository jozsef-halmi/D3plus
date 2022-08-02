using Catalog.Application.Categorys.Commands.DeleteCategory;
using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Catalog.Application.IntegrationTests.Categories.Commands;

using static Testing;

public class DeleteCategoryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteCategory()
    {
        var category = await AddAsync(new Category() { Name = "New category" });

        var deleteCommand = new DeleteCategoryCommand
        {
            Id = category.Id
        };

        var deletedCategoryId = await SendAsync(deleteCommand);

        var deletedCategory = await FindAsync<Category>(category.Id);

        deletedCategory.Should().BeNull();
        category.Id.Should().Be(deletedCategoryId);

        (await FindAsync<Category>(deletedCategoryId)).Should().BeNull();
    }

    [Test]
    public async Task ShouldThrowNotFound()
    {
        var command = new DeleteCategoryCommand
        {
            Id = 9999
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteProducts()
    {
        var category = await AddAsync(new Category() { Name = "New category" });

        var product = await AddAsync(new Domain.Entities.Product()
        {
            Name = "New product",
            Price = 5.99M,
            Amount = 2,
            CategoryId = category.Id
        });

        var deleteCommand = new DeleteCategoryCommand
        {
            Id = category.Id
        };

        var deletedCategoryId = await SendAsync(deleteCommand);
        var deletedCategory = await FindAsync<Category>(category.Id);
        var products = GetAll<Domain.Entities.Product>();

        deletedCategory.Should().BeNull();
        category.Id.Should().Be(deletedCategoryId);
        products.Should().NotContain(p => p.Id == product.Id);
        (await FindAsync<Category>(deletedCategoryId)).Should().BeNull();
    }
}
