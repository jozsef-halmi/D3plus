using GraphQL.Instrumentation;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class CategoriesSchema : Schema
{
    public CategoriesSchema(IServiceProvider provider)
        : base(provider)
    {
        Query =  (CategoriesQuery)provider.GetService(typeof(CategoriesQuery)) ?? throw new InvalidOperationException();
        Mutation = (CategoriesMutation)provider.GetService(typeof(CategoriesMutation)) ?? throw new InvalidOperationException();
        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}
