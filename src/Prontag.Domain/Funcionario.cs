namespace Prontag.Domain;

public class Funcionario : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public required string Nome { get; set; }
    public bool Ativo { get; set; } = true;
}