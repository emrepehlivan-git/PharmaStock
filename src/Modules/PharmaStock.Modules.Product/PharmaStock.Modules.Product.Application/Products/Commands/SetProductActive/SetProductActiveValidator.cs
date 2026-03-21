using FluentValidation;
using PharmaStock.BuildingBlocks.Audit;

namespace PharmaStock.Modules.Product.Application.Products.Commands.SetProductActive;

public sealed class SetProductActiveValidator : AbstractValidator<SetProductActiveCommand>
{
    public SetProductActiveValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Reason)
            .MaximumLength(ComplianceAuditLogConstants.MaxLength.Reason)
            .When(x => x.Reason is not null);
    }
}
