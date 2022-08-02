using Catalog.GraphQL.GraphQL.Types;
using GraphQL;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class CategoriesMutation : ObjectGraphType
{
    /// <example>
    /// This is an example JSON request for a mutation
    /// {
    ///   "query": "mutation ($category:CategoryInput!){ createCategory(category: $category) { id name } }",
    ///   "variables": {
    ///     "category": {
    ///       "name": "Paper"
    ///     }
    ///   }
    /// }
    /// </example>
    public CategoriesMutation(IServiceProvider serviceProvider)
    {
        Name = "Mutation";

        FieldAsync<CategoryType>(
            "createCategory",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<CreateCategoryInputType>> { Name = "category" }
            ),
            resolve: async context => 
            {
                var category = context.GetArgument<Category>("category");
                using var scope = serviceProvider.CreateScope();
                var services = scope.ServiceProvider;
                var categoriesData = services.GetRequiredService<CategoriesDataLoader>();
                return await categoriesData.AddCategory(category);
            });

        FieldAsync<CategoryType>(
           "updateCategory",
           arguments: new QueryArguments(
               new QueryArgument<NonNullGraphType<UpdateCategoryInputType>> { Name = "category" }
           ),
           resolve: async context =>
           {
               var category = context.GetArgument<Category>("category");
               using var scope = serviceProvider.CreateScope();
               var services = scope.ServiceProvider;
               var categoriesData = services.GetRequiredService<CategoriesDataLoader>();
               return await categoriesData.UpdateCategory(category);
           });

        FieldAsync<CategoryType>(
           "deleteCategory",
           arguments: new QueryArguments(
               new QueryArgument<NonNullGraphType<DeleteCategoryInputType>> { Name = "category" }
           ),
           resolve: async context =>
           {
               var category = context.GetArgument<Category>("category");
               using var scope = serviceProvider.CreateScope();
               var services = scope.ServiceProvider;
               var categoriesData = services.GetRequiredService<CategoriesDataLoader>();
               return await categoriesData.DeleteCategory(category);
           });
    }
}
