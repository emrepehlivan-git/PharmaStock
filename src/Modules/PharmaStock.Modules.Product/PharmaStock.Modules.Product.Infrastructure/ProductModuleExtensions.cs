using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.BuildingBlocks.Repositories;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Infrastructure.Persistence;

namespace PharmaStock.Modules.Product.Infrastructure;

public static class ProductModuleExtensions
{
    public static IServiceCollection AddProductModule(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ProductDbContext>((sp, options) =>
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>()));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork>(sp => new EfUnitOfWork<ProductDbContext>(sp.GetRequiredService<ProductDbContext>()));

        return services;
    }
}
