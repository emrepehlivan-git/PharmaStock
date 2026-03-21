using FluentAssertions;
using FluentValidation;
using PharmaStock.Modules.Product.Application.Products.Queries.GetProductByCode;
using PharmaStock.Modules.Product.Domain.Constants;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Queries.GetProductByCode;

public sealed class GetProductByCodeValidatorTests
{
    private readonly IValidator<GetProductByCodeQuery> _validator = new GetProductByCodeValidator();

    [Fact]
    public void Validate_WhenValid_ReturnsSuccess()
    {
        var query = GetProductByCodeQueryTestData.Query("CODE-001");

        var result = _validator.Validate(query);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenCodeIsEmpty_ReturnsFailure(string code)
    {
        var query = GetProductByCodeQueryTestData.Query(code);

        var result = _validator.Validate(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(GetProductByCodeQuery.Code));
    }

    [Fact]
    public void Validate_WhenCodeExceedsMaxLength_ReturnsFailure()
    {
        var query = GetProductByCodeQueryTestData.Query(new string('x', ProductConstants.MaxLength.Code + 1));

        var result = _validator.Validate(query);

        result.IsValid.Should().BeFalse();
    }
}
