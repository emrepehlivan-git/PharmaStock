using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.DependencyInjection;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Infrastructure.Persistence;

namespace PharmaStock.Modules.Product.Infrastructure;

public static class ProductModuleExtensions
{
    public static IServiceCollection AddProductModule(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = Guard.AgainstNullOrWhiteSpace(configuration.GetConnectionString("DefaultConnection"));
        services.AddDbContext<ProductDbContext>((sp, options) =>
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>()));

        services.AddScannedServices(typeof(ProductRepository).Assembly);

        return services;
    }
}
