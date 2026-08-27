namespace Prontag.Domain;

public class RelatorioMensal : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public required string MesAno { get; set; } // formato "2026-08"
    public int TotalImpressoes { get; set; }
    public Guid? FuncionarioTopId { get; set; }
    public Guid? ProdutoTopId { get; set; }
    public required string BreakdownJson { get; set; }
}