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


            for (int i = 0; i < 12; i++)
            {
                _context.Add(new Product()
                {
                    CategoryId = category.Entity.Id,
                    Name = $"Test product{i}",
                    Description = $"Test description{i}",
                    Price =  i*2M,
                    Amount = i
                });
            }
        }

        await _context.SaveChangesAsync();
    }
}
