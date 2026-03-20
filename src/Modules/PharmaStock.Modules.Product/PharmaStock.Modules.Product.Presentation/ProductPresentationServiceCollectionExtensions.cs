using Microsoft.Extensions.DependencyInjection;
using PharmaStock.BuildingBlocks.DependencyInjection;
using PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;

namespace PharmaStock.Modules.Product.Presentation;

public static class ProductPresentationServiceCollectionExtensions
{
    public static IServiceCollection AddProductPresentation(this IServiceCollection services)
    {
        services.AddScannedServices(typeof(CreateProductCommandHandler).Assembly);
        return services;
    }
}

