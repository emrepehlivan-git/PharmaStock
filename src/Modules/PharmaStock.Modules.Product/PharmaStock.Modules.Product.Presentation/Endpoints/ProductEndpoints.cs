using Microsoft.AspNetCore.Routing;
using PharmaStock.Modules.Product.Presentation.Endpoints.Products;

namespace PharmaStock.Modules.Product.Presentation;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        CreateProductEndpoint.Map(endpoints);
        return endpoints;
    }
}
