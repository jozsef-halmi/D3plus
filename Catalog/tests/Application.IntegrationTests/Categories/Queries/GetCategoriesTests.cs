using Catalog.Application.TodoLists.Queries.GetCategories;
using Catalog.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Catalog.Application.IntegrationTests.TodoLists.Queries;

using static Testing;

public class GetCategoriesTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllCategories()
    {
        await AddAsync(new Category
        {
            Name = "Example Category1",
        });

        await AddAsync(new Category
        {
            Name = "Example Category2",
        });

        var query = new GetCategoriesQuery();

        var result = await SendAsync(query);

        result.Categories.Should().HaveCount(2);
    }

    [Test]
    public async Task ShouldReturnParentCategories()
    {
        // Set up parent
        var parentCategoryName = "Example Category1";
        await AddAsync(new Category
        {
            Name = parentCategoryName,
        });

        var categories = GetAll<Category>();
        var parentCategoryId = categories.First().Id;


        // Set up child
        var childCategoryName = "Example Category2";
        await AddAsync(new Category
        {
            Name = childCategoryName,
            ParentCategoryId = parentCategoryId
        });

        var query = new GetCategoriesQuery();

        var result = await SendAsync(query);

        result.Categories.Should().HaveCount(2);
        var parentCategory = result.Categories.FirstOrDefault(c => c.Name == parentCategoryName);
        var childCategory = result.Categories.FirstOrDefault(c => c.Name == childCategoryName);
        childCategory.ParentCategoryId.Should().NotBeNull();
        childCategory.ParentCategoryId.Should().Be(parentCategoryId);
        childCategory.ParentCategoryName.Should().Be(parentCategoryName);
    }
}
