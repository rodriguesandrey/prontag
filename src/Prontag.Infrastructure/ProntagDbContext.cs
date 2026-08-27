using Microsoft.EntityFrameworkCore;
using Prontag.Domain;

namespace Prontag.Infrastructure;

public class ProntagDbContext : DbContext
{
    private readonly ITenantProvider _tenantProvider;

    public ProntagDbContext(DbContextOptions<ProntagDbContext> options, ITenantProvider tenantProvider)
        : base(options)
    {
        _tenantProvider = tenantProvider;
    }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Impressao> Impressoes => Set<Impressao>();
    public DbSet<RelatorioMensal> RelatoriosMensais => Set<RelatorioMensal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Produto>().HasQueryFilter(p => p.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<Funcionario>().HasQueryFilter(f => f.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<Impressao>().HasQueryFilter(i => i.TenantId == _tenantProvider.TenantId);
        modelBuilder.Entity<RelatorioMensal>().HasQueryFilter(r => r.TenantId == _tenantProvider.TenantId);
    }
}