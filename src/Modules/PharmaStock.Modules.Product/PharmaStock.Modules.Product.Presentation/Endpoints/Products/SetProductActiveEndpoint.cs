using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Commands.SetProductActive;
using PharmaStock.Modules.Product.Presentation.Dtos.Products.SetProductActive;

namespace PharmaStock.Modules.Product.Presentation.Endpoints.Products;

internal static class SetProductActiveEndpoint
{
    internal static void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPatch("/api/products/{id:guid}/active", HandleSetProductActiveAsync)
            .WithName("SetProductActive")
            .WithTags("Products");
    }

    private static async Task<IResult> HandleSetProductActiveAsync(
        Guid id,
        SetProductActiveRequest request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        SetProductActiveCommand command = new(id, request.IsActive, request.Reason);

        Result result = await mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return Results.NoContent();

        if (result.Error == ProductErrors.NotFound)
            return Results.NotFound(new { error = result.Error });

        return Results.BadRequest(new { error = result.Error });
    }
}
