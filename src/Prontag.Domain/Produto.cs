namespace Prontag.Domain;

public class Produto : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public required string Nome { get; set; }
    public int DiasValidade { get; set; }
    public decimal? Preco { get; set; }
    public string? Icone { get; set; }
    public bool Ativo { get; set; } = true;
    public bool UsadoEmProdutos { get; set; } = true;
public bool UsadoEmExpositor { get; set; }
public string Unidade { get; set; } = "kg";
}