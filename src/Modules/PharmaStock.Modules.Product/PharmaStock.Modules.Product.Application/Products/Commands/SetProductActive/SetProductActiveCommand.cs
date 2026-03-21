using Mediator;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Validation;

namespace PharmaStock.Modules.Product.Application.Products.Commands.SetProductActive;

public sealed record SetProductActiveCommand(Guid Id, bool IsActive) : IRequest<Result>, IValidatableRequest;
