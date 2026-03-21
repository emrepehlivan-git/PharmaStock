using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaStock.BuildingBlocks.Audit;
using PharmaStock.BuildingBlocks.Entities;

namespace PharmaStock.Modules.Product.Infrastructure.Persistence.Configurations;

public sealed class ComplianceAuditLogEntryConfiguration : IEntityTypeConfiguration<ComplianceAuditLogEntry>
{
    public void Configure(EntityTypeBuilder<ComplianceAuditLogEntry> builder)
    {
        builder.ToTable("compliance_audit_log_entries");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.AggregateType)
            .HasMaxLength(ComplianceAuditLogConstants.MaxLength.AggregateType)
            .IsRequired();
        builder.Property(e => e.OperationType)
            .HasMaxLength(ComplianceAuditLogConstants.MaxLength.OperationType)
            .IsRequired();
        builder.Property(e => e.PreviousValue);
        builder.Property(e => e.NewValue);
        builder.Property(e => e.Reason).HasMaxLength(ComplianceAuditLogConstants.MaxLength.Reason);
        builder.Property(e => e.PerformedByUserId);
        builder.Property(e => e.OccurredAtUtc).IsRequired();
    }
}
