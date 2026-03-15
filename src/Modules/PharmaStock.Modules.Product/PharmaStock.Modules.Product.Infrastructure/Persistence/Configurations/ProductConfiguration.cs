using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaStock.Modules.Product.Domain.Constants;
using ProductEntity = PharmaStock.Modules.Product.Domain.Entities.Product;

namespace PharmaStock.Modules.Product.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Code).HasMaxLength(ProductConstants.MaxLength.Code).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(ProductConstants.MaxLength.Name).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(ProductConstants.MaxLength.Description);
        builder.Property(e => e.Barcode).HasMaxLength(ProductConstants.MaxLength.Barcode);
        builder.Property(e => e.UnitOfMeasurement).HasMaxLength(ProductConstants.MaxLength.UnitOfMeasurement).IsRequired();
        builder.Property(e => e.Category).HasMaxLength(ProductConstants.MaxLength.Category);
        builder.Property(e => e.Manufacturer).HasMaxLength(ProductConstants.MaxLength.Manufacturer);
        builder.Property(e => e.Brand).HasMaxLength(ProductConstants.MaxLength.Brand);
        builder.Property(e => e.MinimumTemperatureCelsius).HasPrecision(5, 2);
        builder.Property(e => e.MaximumTemperatureCelsius).HasPrecision(5, 2);
        builder.Property(e => e.CriticalStockLevel).HasPrecision(18, 4);
        builder.HasIndex(e => e.Code).IsUnique();
    }
}
