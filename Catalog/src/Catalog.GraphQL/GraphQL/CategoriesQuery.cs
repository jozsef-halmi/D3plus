using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.GraphQL.GraphQL.Types;
using GraphQL;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class CategoriesQuery : ObjectGraphType<object>
{
    public CategoriesQuery(IServiceProvider serviceProvider)
    {
        Name = "Query";

        //FieldAsync<CharacterInterface>("hero", resolve: async context => await data.GetDroidByIdAsync("3"));
        //FieldAsync<HumanType>(
        //    "human",
        //    arguments: new QueryArguments(
        //        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the human" }
        //    ),
        //    resolve: async context => await data.GetHumanByIdAsync(context.GetArgument<string>("id"))
        //);


        //Func<IResolveFieldContext,Task<List<Category>>> func = (context) => data.GetCategories();

        //FieldAsync<ListGraphType<CategoryType>>("categories", resolve: async context => await data.GetCategories());

        FieldAsync<ListGraphType<CategoryType>>("categories", resolve: async context => {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            return await services.GetRequiredService<CategoriesData>().GetCategories();
            });

        //FieldAsync<ListGraphType<CategoryType>>("categories", resolve: async context => {
        //    using var scope = context.RequestServices.CreateScope();
        //    var services = scope.ServiceProvider;
        //    return await services.GetRequiredService<CategoriesData>().GetCategories();
        //}



        //Func<IResolveFieldContext, int, Task<Category>> func = (context, id) => data.GetCategoryByIdAsync(id);

        //FieldDelegate<CategoryType>(
        //    "category",
        //    arguments: new QueryArguments(
        //        new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the category" }
        //    ),
        //    resolve: func
        //);
    }
}