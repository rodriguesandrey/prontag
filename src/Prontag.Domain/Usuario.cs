namespace Prontag.Domain;

public class Usuario : BaseEntity, ITenantEntity
{
    public Guid TenantId { get; set; }
    public required string Nome { get; set; }
    public required string Login { get; set; }
    public required string SenhaHash { get; set; }
    public required string Papel { get; set; } // "Admin" ou "Funcionario"
    public Guid? Token { get; set; }
    public DateTime? TokenExpiraEm { get; set; }
    public bool Ativo { get; set; } = true;
    public bool PedidoRedefinicao { get; set; }
public DateTime? PedidoRedefinicaoEm { get; set; }
}
