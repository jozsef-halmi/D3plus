using GraphQL.Instrumentation;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class ProductsSchema : Schema
{
    public ProductsSchema(IServiceProvider provider) : base(provider)
    {
        Query =  (ProductsQuery)provider.GetService(typeof(ProductsQuery)) ?? throw new InvalidOperationException();
        Mutation = (ProductsMutation)provider.GetService(typeof(ProductsMutation)) ?? throw new InvalidOperationException();
        FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}
