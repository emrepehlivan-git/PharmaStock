using PharmaStock.Modules.Product.Application.Products;
using PharmaStock.Modules.Product.Application.Products.Queries.GetProducts;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Application.Tests.Products.Queries.GetProducts;

internal static class GetProductsQueryTestData
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

    public static GetProductsQuery Default() =>
        new(
            PageNumber: 1,
            PageSize: 10,
            Search: null,
            Category: null,
            IsActive: null,
            SortBy: ProductSortField.Code,
            SortDescending: false);
}
