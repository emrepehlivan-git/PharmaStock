using FluentAssertions;
using FluentValidation;
using PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;
using PharmaStock.Modules.Product.Domain.Constants;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Commands.CreateProduct;

public sealed class CreateProductValidatorTests
{
    private readonly IValidator<CreateProductCommand> _validator = new CreateProductValidator();

    [Fact]
    public void Validate_WhenValid_ReturnsSuccess()
    {
        var cmd = CreateProductCommandTestData.Valid();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenCodeIsEmpty_ReturnsFailure(string code)
    {
        var cmd = CreateProductCommandTestData.New().WithCode(code).Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductCommand.Code));
    }

    [Fact]
    public void Validate_WhenCodeExceedsMaxLength_ReturnsFailure()
    {
        var cmd = CreateProductCommandTestData.New()
            .WithCode(new string('x', ProductConstants.MaxLength.Code + 1))
            .Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenColdChainRequiredButTemperaturesMissing_ReturnsFailure()
    {
        var cmd = CreateProductCommandTestData.New().WithColdChain(required: true, minC: null, maxC: null).Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage == ProductConstants.Messages.ColdChainRequiresTemperatureLimits);
    }

    [Fact]
    public void Validate_WhenColdChainAndMinGreaterThanMax_ReturnsFailure()
    {
        var cmd = CreateProductCommandTestData.New()
            .WithColdChain(required: true, minC: 10m, maxC: 5m)
            .Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenCriticalStockLevelNegative_ReturnsFailure()
    {
        var cmd = CreateProductCommandTestData.New().WithCriticalStockLevel(-1m).Build();

        var result = _validator.Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateProductCommand.CriticalStockLevel));
    }
}
