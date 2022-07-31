using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Infrastructure.GraphQL.Types;

namespace Infrastructure.GraphQL;

public class CategoriesQuery : ObjectGraphType<object>
{
    public CategoriesQuery(CategoriesData data)
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

        Func<IResolveFieldContext, int, Task<Category>> func = (context, id) => data.GetCategoryByIdAsync(id);

        FieldDelegate<CategoryType>(
            "category",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the category" }
            ),
            resolve: func
        );
    }
}