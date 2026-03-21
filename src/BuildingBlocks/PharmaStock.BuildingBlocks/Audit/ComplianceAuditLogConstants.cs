namespace PharmaStock.BuildingBlocks.Audit;

public static class ComplianceAuditLogConstants
{
    public static class MaxLength
    {
        public const int AggregateType = 256;
        public const int OperationType = 128;
        public const int Reason = 2000;
    }
}
