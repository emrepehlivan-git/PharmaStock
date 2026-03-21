using FluentAssertions;
using FluentValidation;
using PharmaStock.Modules.Product.Application.Products.Commands.UpdateProduct;
using PharmaStock.Modules.Product.Domain.Constants;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Commands.UpdateProduct;

public sealed class UpdateProductValidatorTests
{
    private readonly IValidator<UpdateProductCommand> _validator = new UpdateProductValidator();

    [Fact]
    public void Validate_WhenValid_ReturnsSuccess()
    {
        var cmd = UpdateProductCommandTestData.Valid();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenIdIsEmpty_ReturnsFailure()
    {
        var cmd = UpdateProductCommandTestData.New().WithId(Guid.Empty).Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateProductCommand.Id));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenCodeIsEmpty_ReturnsFailure(string code)
    {
        var cmd = UpdateProductCommandTestData.New().WithCode(code).Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateProductCommand.Code));
    }

    [Fact]
    public void Validate_WhenNameIsEmpty_ReturnsFailure()
    {
        var cmd = UpdateProductCommandTestData.New().WithName("").Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateProductCommand.Name));
    }

    [Fact]
    public void Validate_WhenUnitOfMeasurementIsEmpty_ReturnsFailure()
    {
        var cmd = UpdateProductCommandTestData.New().WithUnitOfMeasurement("").Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateProductCommand.UnitOfMeasurement));
    }

    [Fact]
    public void Validate_WhenCodeExceedsMaxLength_ReturnsFailure()
    {
        var cmd = UpdateProductCommandTestData.New()
            .WithCode(new string('x', ProductConstants.MaxLength.Code + 1))
            .Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenDescriptionExceedsMaxLength_ReturnsFailure()
    {
        var cmd = UpdateProductCommandTestData.New()
            .WithDescription(new string('d', ProductConstants.MaxLength.Description + 1))
            .Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateProductCommand.Description));
    }

    [Fact]
    public void Validate_WhenColdChainRequiredButTemperaturesMissing_ReturnsFailure()
    {
        var cmd = UpdateProductCommandTestData.New().WithColdChain(required: true, minC: null, maxC: null).Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ProductConstants.Messages.ColdChainRequiresTemperatureLimits);
    }

    [Fact]
    public void Validate_WhenColdChainAndMinGreaterThanMax_ReturnsFailure()
    {
        var cmd = UpdateProductCommandTestData.New()
            .WithColdChain(required: true, minC: 10m, maxC: 5m)
            .Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenCriticalStockLevelNegative_ReturnsFailure()
    {
        var cmd = UpdateProductCommandTestData.New().WithCriticalStockLevel(-1m).Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateProductCommand.CriticalStockLevel));
    }
}
