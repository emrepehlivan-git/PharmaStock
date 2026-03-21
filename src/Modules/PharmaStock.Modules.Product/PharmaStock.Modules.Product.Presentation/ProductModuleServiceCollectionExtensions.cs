using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PharmaStock.BuildingBlocks.DependencyInjection;
using PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;
using PharmaStock.Modules.Product.Infrastructure;

namespace PharmaStock.Modules.Product.Presentation;

public static class ProductModuleServiceCollectionExtensions
{
    public static IServiceCollection AddProductModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddProductInfrastructure(configuration);
        services.AddScannedServices(typeof(CreateProductCommandHandler).Assembly);
        return services;
    }
}
