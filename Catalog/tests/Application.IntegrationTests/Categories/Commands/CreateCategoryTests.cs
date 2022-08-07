using Catalog.Application.Categorys.Commands.CreateCategory;
using Catalog.Application.Common.Exceptions;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Catalog.Application.IntegrationTests.Categories.Commands;

using static Testing;

public class CreateCategoryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateCategoryCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateCategory()
    {
        var command = new CreateCategoryCommand
        {
            Name = "New category"
        };

        var categoryId = await SendAsync(command);

        var category = await FindAsync<Category>(categoryId);

        category.Should().NotBeNull();
        category.Name.Should().Be(command.Name);
        category.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        category.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }

    [Test]
    public async Task ShouldCreateCategoryWithParent()
    {
        var createParentCategoryCommand = new CreateCategoryCommand
        {
            Name = "Parent category",
            ImageUrl = new Uri("http://localhost/example-parent.jpg")
        };

        var parentCategoryId = await SendAsync(createParentCategoryCommand);

        var createCategoryCommand = new CreateCategoryCommand
        {
            Name = "Child category",
            ImageUrl = new Uri("http://localhost/example-child.jpg"),
            ParentCategoryId = parentCategoryId
        };

        var categoryId = await SendAsync(createCategoryCommand);

        var category = await FindAsync<Category>(categoryId);

        category.Should().NotBeNull();
        category.Name.Should().Be(createCategoryCommand.Name);
        category.ImageUrl.Should().Be(createCategoryCommand.ImageUrl);
        category.ParentCategoryId.Should().Be(createCategoryCommand.ParentCategoryId);
        category.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        category.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
