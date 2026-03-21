using Microsoft.AspNetCore.Routing;
using PharmaStock.Modules.Product.Presentation.Endpoints.Products;

namespace PharmaStock.Modules.Product.Presentation;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        CreateProductEndpoint.Map(endpoints);
        GetProductsEndpoint.Map(endpoints);
        GetProductByCodeEndpoint.Map(endpoints);
        GetProductByIdEndpoint.Map(endpoints);
        UpdateProductEndpoint.Map(endpoints);
        SetProductActiveEndpoint.Map(endpoints);
        return endpoints;
    }
}
