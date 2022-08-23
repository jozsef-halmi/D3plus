using Carting.WebApi.Application.Common.Configuration;
using Carting.WebApi.Application.Common.Interfaces;
using LiteDB;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;

namespace Carting.WebApi.Infrastructure.Persistence;

public class CartingDbContext : ICartingDbContext
{
    private readonly IOptions<PersistenceOptions> _persistenceConfiguration;
    private readonly TelemetryConfiguration _telemetryConfiguration;

    public CartingDbContext(IOptions<PersistenceOptions> persistenceConfiguration, IConfiguration configuration)
    {
        _persistenceConfiguration = persistenceConfiguration;
        _telemetryConfiguration = TelemetryConfiguration.CreateDefault();
        _telemetryConfiguration.ConnectionString = configuration["ApplicationInsights:ConnectionString"];
    }

    public T Get<T>(string id)
    {
        var telemetryClient = new TelemetryClient(_telemetryConfiguration);
        using (var operation = telemetryClient.StartOperation<DependencyTelemetry>("LiteDb"))
        {
            operation.Telemetry.Type = "NoSQL";
            using var db = new LiteDatabase(_persistenceConfiguration.Value.ConnectionString);

            var col = db.GetCollection<T>(typeof(T).Name);
            var result = col.FindById(id);

            return result;
        }

    }

    public IEnumerable<T> GetAll<T>(Func<T, bool> pred)
    {
        var telemetryClient = new TelemetryClient(_telemetryConfiguration);
        using (var operation = telemetryClient.StartOperation<DependencyTelemetry>("LiteDb"))
        {
            operation.Telemetry.Type = "NoSQL";
            using var db = new LiteDatabase(_persistenceConfiguration.Value.ConnectionString);

            var col = db.GetCollection<T>(typeof(T).Name);

            return col.FindAll().Where(pred).ToList();
        }

    }

    public void Insert<T>(T entity, CancellationToken cancellationToken)
    {
        var telemetryClient = new TelemetryClient(_telemetryConfiguration);
        using (var operation = telemetryClient.StartOperation<DependencyTelemetry>("LiteDb"))
        {
            operation.Telemetry.Type = "NoSQL";
            using var db = new LiteDatabase(_persistenceConfiguration.Value.ConnectionString);

            var col = db.GetCollection<T>(typeof(T).Name);
            col.Insert(entity);
        }

    }

    public void Update<T>(T entity)
    {
        var telemetryClient = new TelemetryClient(_telemetryConfiguration);
        using (var operation = telemetryClient.StartOperation<DependencyTelemetry>("LiteDb"))
        {
            operation.Telemetry.Type = "NoSQL";
            using var db = new LiteDatabase(_persistenceConfiguration.Value.ConnectionString);

            var col = db.GetCollection<T>(typeof(T).Name);
            col.Update(entity);
        }

    }
}
