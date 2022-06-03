using Catalog.Application.Common.Exceptions;
using Catalog.Application.Categorys.Commands.CreateCategory;
using Catalog.Application.Categorys.Commands.CreateCategory;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Catalog.Application.IntegrationTests.Categorys.Commands;

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
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateCategoryCommand
        {
            Name = "New category"
        };

        var categoryId = await SendAsync(command);

        var category = await FindAsync<Category>(categoryId);

        category.Should().NotBeNull();
        category.Name.Should().Be(command.Name);
        category.CreatedBy.Should().Be(userId);
        category.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        category.LastModifiedBy.Should().Be(userId);
        category.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
