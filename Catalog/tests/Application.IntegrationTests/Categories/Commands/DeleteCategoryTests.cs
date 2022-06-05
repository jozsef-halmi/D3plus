using Catalog.Application.Common.Exceptions;
using Catalog.Application.Categorys.Commands.DeleteCategory;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Catalog.Application.Categorys.Commands.CreateCategory;

namespace Catalog.Application.IntegrationTests.Categories.Commands;

using static Testing;

public class DeleteCategoryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteCategory()
    {
        var createCommand = new CreateCategoryCommand
        {
            Name = "New category"
        };

        var categoryId = await SendAsync(createCommand);

        var deleteCommand = new DeleteCategoryCommand
        {
            CategoryId = categoryId
        };

        var deletedCategoryId = await SendAsync(deleteCommand);

        var category = await FindAsync<Category>(categoryId);

        category.Should().BeNull();
        categoryId.Should().Be(deletedCategoryId);

        (await FindAsync<Category>(deletedCategoryId)).Should().BeNull();
    }

    [Test]
    public async Task ShouldThrowNotFound()
    {
        var command = new DeleteCategoryCommand
        {
            CategoryId = 9999
        };

        Func<Task> act = async () => await SendAsync(command);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
