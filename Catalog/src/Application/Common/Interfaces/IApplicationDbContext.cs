using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }

    DbSet<Domain.Entities.Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
