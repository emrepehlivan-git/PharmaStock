using Microsoft.Extensions.DependencyInjection;

namespace PharmaStock.BuildingBlocks.Audit;

public static class AuditServiceCollectionExtensions
{
    public static IServiceCollection AddAuditableEntityInterceptor(this IServiceCollection services)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        return services;
    }

    public static IServiceCollection AddAuditUserAccessor(this IServiceCollection services)
    {
        services.AddScoped<IAuditUserAccessor, HttpContextAuditUserAccessor>();
        return services;
    }
}
