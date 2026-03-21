using Mediator;
using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Validation;

namespace PharmaStock.Modules.Product.Application.Products.Queries.GetProductByCode;

public sealed record GetProductByCodeQuery(string Code) : IRequest<Result<ProductDto>>, IValidatableRequest;
