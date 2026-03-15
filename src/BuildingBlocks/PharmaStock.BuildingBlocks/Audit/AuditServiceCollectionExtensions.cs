using Microsoft.Extensions.DependencyInjection;

namespace PharmaStock.BuildingBlocks.Audit;

public static class AuditServiceCollectionExtensions
{
    public static IServiceCollection AddAuditableEntityInterceptor(this IServiceCollection services)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        return services;
    }
}
