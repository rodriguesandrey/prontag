namespace Prontag.Infrastructure;

public class FixedTenantProvider : ITenantProvider
{
    public Guid TenantId { get; } = Guid.Parse("11111111-1111-1111-1111-111111111111");
}