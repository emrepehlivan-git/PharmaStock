using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PharmaStock.Modules.Product.Infrastructure.Persistence;

public sealed class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=PharmaStock;Username=postgres;Password=postgres")
            .UseSnakeCaseNamingConvention();
        return new ProductDbContext(optionsBuilder.Options);
    }
}
