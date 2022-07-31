using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace Catalog.GraphQL.GraphQL;

public class CategoriesSchema : Schema
{
    public CategoriesSchema(IServiceProvider provider)
        : base(provider)
    {
        Query =  (CategoriesQuery)provider.GetService(typeof(CategoriesQuery)) ?? throw new InvalidOperationException();
        //Query = (CategoriesQuery)provider.GetService(typeof(CategoriesQuery)) ?? throw new InvalidOperationException();
        //Mutation = (StarWarsMutation)provider.GetService(typeof(StarWarsMutation)) ?? throw new InvalidOperationException();

        //FieldMiddleware.Use(new InstrumentFieldsMiddleware());
    }
}
