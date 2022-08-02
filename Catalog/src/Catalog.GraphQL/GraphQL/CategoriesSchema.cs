using GraphQL.Instrumentation;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class CategoriesSchema : Schema
{
    public CategoriesSchema(IServiceProvider provider)
        : base(provider)
    {
        Query = provider.GetService(typeof(CategoriesQuery)) as CategoriesQuery ?? throw new InvalidOperationException();
        Mutation = provider.GetService(typeof(CategoriesMutation)) as CategoriesMutation ?? throw new InvalidOperationException();
        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}
