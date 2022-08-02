using Catalog.Application.Outbox;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }

    DbSet<Domain.Entities.Product> Products { get; }

    DbSet<OutboxMessage> OutboxMessages { get; }

    Task StartTransaction();

    Task Commit();

    Task Rollback();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
