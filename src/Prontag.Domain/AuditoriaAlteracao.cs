namespace Prontag.Domain;

public class AuditoriaAlteracao : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public required string Entidade { get; set; }
    public required string Acao { get; set; }
    public Guid EntidadeId { get; set; }
    public Guid FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
