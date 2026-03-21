using FluentAssertions;
using FluentValidation;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.Modules.Product.Application.Products.Commands.SetProductActive;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Commands.SetProductActive;

public sealed class SetProductActiveValidatorTests
{
    private readonly IValidator<SetProductActiveCommand> _validator = new SetProductActiveValidator();

    [Fact]
    public void Validate_WhenIdIsEmpty_ReturnsFailure()
    {
        var cmd = new SetProductActiveCommand(Guid.Empty, true, null);

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(SetProductActiveCommand.Id));
    }

    [Fact]
    public void Validate_WhenReasonExceedsMaxLength_ReturnsFailure()
    {
        var cmd = new SetProductActiveCommand(Guid.NewGuid(), true, new string('x', ComplianceAuditLogConstants.MaxLength.Reason + 1));

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(SetProductActiveCommand.Reason));
    }
}
