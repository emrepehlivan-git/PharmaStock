using PharmaStock.Modules.Product.Application.Products.Commands.SetProductActive;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Commands.SetProductActive;

internal static class SetProductActiveCommandTestData
{
    public static ProductEntity ExistingProduct() =>
        ProductEntity.Create(
            code: "CODE-001",
            name: "Ürün adı",
            description: null,
            barcode: null,
            unitOfMeasurement: "Kutu",
            category: null,
            manufacturer: null,
            brand: null,
            batchTrackingEnabled: false,
            expirationTrackingEnabled: false,
            serialTrackingEnabled: false,
            coldChainRequired: false,
            minimumTemperatureCelsius: null,
            maximumTemperatureCelsius: null,
            criticalStockLevel: null);

    public static SetProductActiveCommand With(Guid id, bool isActive) => new(id, isActive);
}
