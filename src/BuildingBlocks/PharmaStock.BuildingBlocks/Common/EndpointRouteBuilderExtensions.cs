using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace PharmaStock.BuildingBlocks.Common;

public static class EndpointRouteBuilderExtensions
{
    private static readonly Type[] MapParameterTypes = [typeof(IEndpointRouteBuilder)];

    public static IEndpointRouteBuilder MapEndpointsFromAssemblyContaining<T>(this IEndpointRouteBuilder endpoints)
        => endpoints.MapEndpointsFromAssembly(typeof(T).Assembly);

    public static IEndpointRouteBuilder MapEndpointsFromAssemblies(
        this IEndpointRouteBuilder endpoints,
        params Assembly[] assemblies)
    {
        foreach (Assembly assembly in assemblies)
            endpoints.MapEndpointsFromAssembly(assembly);

        return endpoints;
    }

    public static IEndpointRouteBuilder MapEndpointsFromAssembly(
        this IEndpointRouteBuilder endpoints,
        Assembly assembly)
    {
        foreach (Type endpointType in GetEndpointTypes(assembly))
        {
            MethodInfo? map = endpointType.GetMethod(
                "Map",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                types: MapParameterTypes,
                modifiers: null);

            if (map is null)
                continue;

            map.Invoke(null, [endpoints]);
        }

        return endpoints;
    }

    private static IEnumerable<Type> GetEndpointTypes(Assembly assembly)
    {
        Type[] types;
        try
        {
            types = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            types = ex.Types.Where(t => t is not null).Cast<Type>().ToArray();
        }

        return types
            .Where(t =>
                t.IsClass &&
                t.IsAbstract &&
                t.IsSealed &&
                t.Name.EndsWith("Endpoint", StringComparison.Ordinal))
            .OrderBy(t => t.FullName, StringComparer.Ordinal);
    }
}
