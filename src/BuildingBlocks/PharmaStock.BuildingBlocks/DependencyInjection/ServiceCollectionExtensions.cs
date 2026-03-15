using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace PharmaStock.BuildingBlocks.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScannedServices(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        ServiceLifetime defaultLifetime = ServiceLifetime.Scoped)
    {
        var assemblyList = assemblies.ToList();
        if (assemblyList.Count == 0)
            return services;

        static bool HasNoLifetimeMarker(Type t) =>
            !typeof(ISingletonLifetime).IsAssignableFrom(t) &&
            !typeof(ITransientLifetime).IsAssignableFrom(t) &&
            !typeof(IScopedLifetime).IsAssignableFrom(t);

        services.Scan(scan => scan
            .FromAssemblies(assemblyList)
            .AddClasses(classes => classes.Where(t => typeof(ISingletonLifetime).IsAssignableFrom(t)))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .As(GetServiceTypesFor)
            .WithSingletonLifetime()
            .AddClasses(classes => classes.Where(t => typeof(ITransientLifetime).IsAssignableFrom(t)))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .As(GetServiceTypesFor)
            .WithTransientLifetime()
            .AddClasses(classes => classes.Where(t => typeof(IScopedLifetime).IsAssignableFrom(t)))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .As(GetServiceTypesFor)
            .WithScopedLifetime()
            .AddClasses(classes => classes.Where(HasNoLifetimeMarker))
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .As(GetServiceTypesFor)
            .WithLifetime(defaultLifetime));

        return services;
    }

    public static IServiceCollection AddScannedServices(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime defaultLifetime = ServiceLifetime.Scoped)
        => services.AddScannedServices([assembly], defaultLifetime);

    private static IEnumerable<Type> GetServiceTypesFor(Type implementationType)
    {
        var matchingInterface = implementationType.GetInterfaces()
            .FirstOrDefault(i => i.Name == "I" + implementationType.Name);
        return matchingInterface is not null
            ? [matchingInterface]
            : [implementationType];
    }
}
