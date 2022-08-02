using GraphQL.Instrumentation;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class ProductsSchema : Schema
{
    public ProductsSchema(IServiceProvider provider) : base(provider)
    {
        Query = provider.GetService(typeof(ProductsQuery)) as ProductsQuery ?? throw new InvalidOperationException();
        Mutation = provider.GetService(typeof(ProductsMutation)) as ProductsMutation ?? throw new InvalidOperationException();
        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}
