using PharmaStock.Modules.Product.Application.Products.Queries.GetProductByCode;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Queries.GetProductByCode;

internal static class GetProductByCodeQueryTestData
{
    public static ProductEntity SampleProduct() =>
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

    public static GetProductByCodeQuery Query(string code) => new(code);
}
