using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.Categories.Any())
        {
            var category = _context.Add(new Category()
            {
                Name = "Test category1",
            });

            _context.Add(new Category()
            {
                Name = "Test category2",
            });

            await _context.SaveChangesAsync();

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product1",
                Description = "Test description1",
                Price = 5.99M,
                Amount = 2
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product3",
                Description = "Test description3",
                Price = 3.99M,
                Amount = 3
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product4",
                Description = "Test description4",
                Price = 5.99M,
                Amount = 2
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product5",
                Description = "Test description5",
                Price = 3.99M,
                Amount = 3
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product6",
                Description = "Test description6",
                Price = 5.99M,
                Amount = 2
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product7",
                Description = "Test description7",
                Price = 3.99M,
                Amount = 3
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product8",
                Description = "Test description8",
                Price = 5.99M,
                Amount = 2
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product9",
                Description = "Test description9",
                Price = 3.99M,
                Amount = 3
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product10",
                Description = "Test description10",
                Price = 5.99M,
                Amount = 2
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product11",
                Description = "Test description11",
                Price = 3.99M,
                Amount = 3
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product12",
                Description = "Test description12",
                Price = 5.99M,
                Amount = 2
            });

            _context.Add(new Product()
            {
                CategoryId = category.Entity.Id,
                Name = "Test product13",
                Description = "Test description13",
                Price = 3.99M,
                Amount = 3
            });
        }

        await _context.SaveChangesAsync();
    }
}
