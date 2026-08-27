namespace Prontag.Domain;

public class Impressao : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public required TipoEtiqueta Tipo { get; set; }

    public Guid ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public Guid? FuncionarioId { get; set; }
    public Funcionario? Funcionario { get; set; }

    public DateOnly Fabricacao { get; set; }
    public DateOnly Vencimento { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}