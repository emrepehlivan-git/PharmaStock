using PharmaStock.BuildingBlocks.Common;
using PharmaStock.BuildingBlocks.Entities;
using PharmaStock.BuildingBlocks.Exceptions;
using PharmaStock.Modules.Product.Domain.Constants;

namespace PharmaStock.Modules.Product.Domain.Entities;

public sealed class Product : AuditableEntityBase
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Barcode { get; private set; }
    public string UnitOfMeasurement { get; private set; } = string.Empty;
    public string? Category { get; private set; }
    public string? Manufacturer { get; private set; }
    public string? Brand { get; private set; }
    public bool BatchTrackingEnabled { get; private set; }
    public bool ExpirationTrackingEnabled { get; private set; }
    public bool SerialTrackingEnabled { get; private set; }
    public bool ColdChainRequired { get; private set; }
    public decimal? MinimumTemperatureCelsius { get; private set; }
    public decimal? MaximumTemperatureCelsius { get; private set; }
    public decimal? CriticalStockLevel { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Product() { }

    public static Product Create(
        string code,
        string name,
        string? description,
        string? barcode,
        string unitOfMeasurement,
        string? category,
        string? manufacturer,
        string? brand,
        bool batchTrackingEnabled,
        bool expirationTrackingEnabled,
        bool serialTrackingEnabled,
        bool coldChainRequired,
        decimal? minimumTemperatureCelsius,
        decimal? maximumTemperatureCelsius,
        decimal? criticalStockLevel)
    {
        var product = new Product
        {
            Code = Guard.AgainstNullOrWhiteSpace(code).Trim(),
            Name = Guard.AgainstNullOrWhiteSpace(name).Trim(),
            Description = description?.Trim(),
            Barcode = barcode?.Trim(),
            UnitOfMeasurement = Guard.AgainstNullOrWhiteSpace(unitOfMeasurement).Trim(),
            Category = category?.Trim(),
            Manufacturer = manufacturer?.Trim(),
            Brand = brand?.Trim(),
            BatchTrackingEnabled = batchTrackingEnabled,
            ExpirationTrackingEnabled = expirationTrackingEnabled,
            SerialTrackingEnabled = serialTrackingEnabled,
            ColdChainRequired = coldChainRequired,
            MinimumTemperatureCelsius = minimumTemperatureCelsius,
            MaximumTemperatureCelsius = maximumTemperatureCelsius,
            CriticalStockLevel = criticalStockLevel,
            IsActive = true
        };

        product.EnsureColdChainTemperatureLimits();
        return product;
    }

    public void Update(
        string code,
        string name,
        string? description,
        string? barcode,
        string unitOfMeasurement,
        string? category,
        string? manufacturer,
        string? brand,
        bool batchTrackingEnabled,
        bool expirationTrackingEnabled,
        bool serialTrackingEnabled,
        bool coldChainRequired,
        decimal? minimumTemperatureCelsius,
        decimal? maximumTemperatureCelsius,
        decimal? criticalStockLevel)
    {
        Code = Guard.AgainstNullOrWhiteSpace(code).Trim();
        Name = Guard.AgainstNullOrWhiteSpace(name).Trim();
        Description = description?.Trim();
        Barcode = barcode?.Trim();
        UnitOfMeasurement = Guard.AgainstNullOrWhiteSpace(unitOfMeasurement).Trim();
        Category = category?.Trim();
        Manufacturer = manufacturer?.Trim();
        Brand = brand?.Trim();
        BatchTrackingEnabled = batchTrackingEnabled;
        ExpirationTrackingEnabled = expirationTrackingEnabled;
        SerialTrackingEnabled = serialTrackingEnabled;
        ColdChainRequired = coldChainRequired;
        MinimumTemperatureCelsius = minimumTemperatureCelsius;
        MaximumTemperatureCelsius = maximumTemperatureCelsius;
        CriticalStockLevel = criticalStockLevel;

        EnsureColdChainTemperatureLimits();
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    private void EnsureColdChainTemperatureLimits()
    {
        if (ColdChainRequired && (MinimumTemperatureCelsius is null || MaximumTemperatureCelsius is null))
            throw new DomainRuleException(ProductConstants.Messages.ColdChainRequiresTemperatureLimits);
    }
}
