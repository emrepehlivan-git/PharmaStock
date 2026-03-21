using Mediator;
using PharmaStock.BuildingBlocks.Common;

namespace PharmaStock.Modules.Product.Application.Products.Commands.SetProductActive;

public sealed record SetProductActiveCommand(Guid Id, bool IsActive) : IRequest<Result>;
