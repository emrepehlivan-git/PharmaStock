using PharmaStock.BuildingBlocks.Common;

namespace PharmaStock.Modules.Product.Domain.Constants;

public static class ProductCodeNormalizer
{
    public static string Normalize(string code) => Guard.AgainstNullOrWhiteSpace(code).Trim();
}
