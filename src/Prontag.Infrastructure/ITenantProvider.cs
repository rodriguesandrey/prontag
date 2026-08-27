namespace Prontag.Infrastructure;

public interface ITenantProvider
{
    Guid TenantId { get; }
}