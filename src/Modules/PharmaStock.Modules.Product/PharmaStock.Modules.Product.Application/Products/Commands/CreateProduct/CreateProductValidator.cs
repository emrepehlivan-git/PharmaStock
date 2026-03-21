using FluentValidation;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.Modules.Product.Domain.Constants;

namespace PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;

public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(ProductConstants.MaxLength.Code);

        RuleFor(x => x.Reason)
            .MaximumLength(ComplianceAuditLogConstants.MaxLength.Reason)
            .When(x => x.Reason is not null);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ProductConstants.MaxLength.Name);

        RuleFor(x => x.UnitOfMeasurement)
            .NotEmpty()
            .MaximumLength(ProductConstants.MaxLength.UnitOfMeasurement);

        RuleFor(x => x.Description)
            .MaximumLength(ProductConstants.MaxLength.Description)
            .When(x => x.Description is not null);

        RuleFor(x => x.Barcode)
            .MaximumLength(ProductConstants.MaxLength.Barcode)
            .When(x => x.Barcode is not null);

        RuleFor(x => x.Category)
            .MaximumLength(ProductConstants.MaxLength.Category)
            .When(x => x.Category is not null);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(ProductConstants.MaxLength.Manufacturer)
            .When(x => x.Manufacturer is not null);

        RuleFor(x => x.Brand)
            .MaximumLength(ProductConstants.MaxLength.Brand)
            .When(x => x.Brand is not null);

        RuleFor(x => x.CriticalStockLevel)
            .GreaterThanOrEqualTo(0)
            .When(x => x.CriticalStockLevel.HasValue);

        RuleFor(x => x)
            .Must(x => x.MinimumTemperatureCelsius.HasValue && x.MaximumTemperatureCelsius.HasValue)
            .WithMessage(ProductConstants.Messages.ColdChainRequiresTemperatureLimits)
            .When(x => x.ColdChainRequired);

        RuleFor(x => x)
            .Must(x => x.MinimumTemperatureCelsius <= x.MaximumTemperatureCelsius)
            .WithMessage(x => string.Format(
                ProductConstants.Messages.InvalidTemperatureRange,
                x.MinimumTemperatureCelsius,
                x.MaximumTemperatureCelsius))
            .When(x => x.ColdChainRequired && x.MinimumTemperatureCelsius.HasValue && x.MaximumTemperatureCelsius.HasValue);
    }
}
