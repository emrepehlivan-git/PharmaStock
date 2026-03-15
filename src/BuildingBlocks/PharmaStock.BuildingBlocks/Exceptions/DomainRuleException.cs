namespace PharmaStock.BuildingBlocks.Exceptions;

public sealed class DomainRuleException : DomainException
{
    public DomainRuleException(string message) : base(message) { }

    public DomainRuleException(string message, Exception innerException) : base(message, innerException) { }
}
