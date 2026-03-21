using PharmaStock.Modules.Product.Application.Products.Commands.CreateProduct;
using PharmaStock.Modules.Product.Domain.Constants;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Commands.CreateProduct;

internal static class CreateProductCommandTestData
{
    public static CreateProductCommand Valid() => new CommandBuilder().Build();

    public static CommandBuilder New() => new();

    internal sealed class CommandBuilder
    {
        private string _code = "CODE-001";
        private string _name = "Ürün adı";
        private string? _description;
        private string _unitOfMeasurement = "Kutu";
        private bool _coldChainRequired;
        private decimal? _minimumTemperatureCelsius;
        private decimal? _maximumTemperatureCelsius;
        private decimal? _criticalStockLevel;

        public CommandBuilder WithCode(string code)
        {
            _code = code;
            return this;
        }

        public CommandBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CommandBuilder WithUnitOfMeasurement(string unit)
        {
            _unitOfMeasurement = unit;
            return this;
        }

        public CommandBuilder WithDescription(string? value)
        {
            _description = value;
            return this;
        }

        public CommandBuilder WithColdChain(bool required, decimal? minC, decimal? maxC)
        {
            _coldChainRequired = required;
            _minimumTemperatureCelsius = minC;
            _maximumTemperatureCelsius = maxC;
            return this;
        }

        public CommandBuilder WithCriticalStockLevel(decimal? value)
        {
            _criticalStockLevel = value;
            return this;
        }

        public CreateProductCommand Build() =>
            new(
                Code: _code,
                Name: _name,
                Description: _description,
                Barcode: null,
                UnitOfMeasurement: _unitOfMeasurement,
                Category: null,
                Manufacturer: null,
                Brand: null,
                BatchTrackingEnabled: false,
                ExpirationTrackingEnabled: false,
                SerialTrackingEnabled: false,
                ColdChainRequired: _coldChainRequired,
                MinimumTemperatureCelsius: _minimumTemperatureCelsius,
                MaximumTemperatureCelsius: _maximumTemperatureCelsius,
                CriticalStockLevel: _criticalStockLevel,
                Reason: null);
    }
}
