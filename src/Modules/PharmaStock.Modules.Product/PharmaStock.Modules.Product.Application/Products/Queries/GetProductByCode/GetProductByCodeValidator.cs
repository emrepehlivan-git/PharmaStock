using FluentValidation;
using PharmaStock.Modules.Product.Domain.Constants;

namespace PharmaStock.Modules.Product.Application.Products.Queries.GetProductByCode;

public sealed class GetProductByCodeValidator : AbstractValidator<GetProductByCodeQuery>
{
    public GetProductByCodeValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(ProductConstants.MaxLength.Code);
    }
}
