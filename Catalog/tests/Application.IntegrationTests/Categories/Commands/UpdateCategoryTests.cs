using Catalog.Application.Common.Exceptions;
using Catalog.Application.Categorys.Commands.UpdateCategory;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Catalog.Application.Categorys.Commands.CreateCategory;

namespace Catalog.Application.IntegrationTests.Categories.Commands;

using static Testing;

public class UpdateCategoryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new UpdateCategoryCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldUpdateCategory()
    {
        var newCategory = await AddAsync(new Category() { Name = "New category", ImageUrl = new Uri("http://localhost/example.jpg") });
        var categoryId = newCategory.Id;

        var category = await FindAsync<Category>(categoryId);

        var updateCommand = new UpdateCategoryCommand()
        {
            CategoryId = categoryId,
            Name = "New category modified",
            ImageUrl = new Uri("http://localhost/example-modified.jpg"),
        };

        var updatedCategoryId = await SendAsync(updateCommand);
        var updatedCategory = await FindAsync<Category>(updatedCategoryId);


        category.Should().NotBeNull();
        updatedCategory.Should().NotBeNull();
        updatedCategoryId.Should().Be(categoryId);


        updatedCategory.Name.Should().Be(updateCommand.Name);
        updatedCategory.ImageUrl.Should().Be(updateCommand.ImageUrl);

        updatedCategory.CreatedBy.Should().Be(category.CreatedBy);
        updatedCategory.Created.Should().Be(category.Created);

        updatedCategory.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }

    [Test]
    public async Task ShouldThrowNotFound()
    {
        var updateParentCategoryCommand = new UpdateCategoryCommand
        {
            CategoryId = 9999
        };

        Func<Task> act = async () => await SendAsync(updateParentCategoryCommand);
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldUpdateParentCategory()
    {
        var createCommand1 = new CreateCategoryCommand
        {
            Name = "New category1",
            ImageUrl = new Uri("http://localhost/example1.jpg"),
        };

        var createCommand2 = new CreateCategoryCommand
        {
            Name = "New category2",
            ImageUrl = new Uri("http://localhost/example2.jpg"),
        };

        var category1Id = await SendAsync(createCommand1);
        var category2Id = await SendAsync(createCommand2);

        var category1 = await FindAsync<Category>(category1Id);

        var updateCommand = new UpdateCategoryCommand()
        {
            CategoryId = category1Id,
            ParentCategoryId = category2Id,
            Name = "New category modified",
            ImageUrl = new Uri("http://localhost/example-modified.jpg"),
        };

        var updatedCategoryId = await SendAsync(updateCommand);
        var updatedCategory = await FindAsync<Category>(updatedCategoryId);


        updatedCategory.Should().NotBeNull();
        updatedCategory.ParentCategoryId.Should().Be(category2Id);
        updatedCategory.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
