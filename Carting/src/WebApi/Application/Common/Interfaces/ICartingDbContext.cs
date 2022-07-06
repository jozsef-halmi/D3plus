namespace Carting.WebApi.Application.Common.Interfaces;

public interface ICartingDbContext
{
    T Get<T>(string id);
    IEnumerable<T> GetAll<T>(Func<T, bool> pred);
    void Update<T>(T entity);
    void Insert<T>(T entity, CancellationToken cancellationToken = default);
}
