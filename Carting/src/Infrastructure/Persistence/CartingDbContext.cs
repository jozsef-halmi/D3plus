using Carting.Application.Common.Configuration;
using Carting.Application.Common.Interfaces;
using LiteDB;
using Microsoft.Extensions.Options;

namespace Carting.Infrastructure.Persistence;

public class CartingDbContext : ICartingDbContext
{
    private readonly IOptions<PersistenceOptions> _persistenceConfiguration;
    
    public CartingDbContext(IOptions<PersistenceOptions> persistenceConfiguration)
    {
        _persistenceConfiguration = persistenceConfiguration;
    }

    public T Get<T>(string id)
    {
        using var db = new LiteDatabase(_persistenceConfiguration.Value.ConnectionString);

        var col = db.GetCollection<T>(typeof(T).Name);
        var result = col.FindById(id);

        return result;
    }

    public void Insert<T>(T entity, CancellationToken cancellationToken)
    {
        using var db = new LiteDatabase(_persistenceConfiguration.Value.ConnectionString);

        var col = db.GetCollection<T>(typeof(T).Name);
        col.Insert(entity);
    }

    public void Update<T>(T entity)
    {
        using var db = new LiteDatabase(_persistenceConfiguration.Value.ConnectionString);

        var col = db.GetCollection<T>(typeof(T).Name);
        col.Update(entity);
    }
}
