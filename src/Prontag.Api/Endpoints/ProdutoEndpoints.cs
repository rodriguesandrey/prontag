using Microsoft.EntityFrameworkCore;
using Prontag.Domain;
using Prontag.Infrastructure;

namespace Prontag.Api.Endpoints;

public static class ProdutoEndpoints
{
    public static void MapProdutoEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/produtos");

        grupo.MapGet("/", async (ProntagDbContext db) =>
            await db.Produtos.Where(p => p.Ativo).ToListAsync());

        grupo.MapPost("/", async (Produto produto, Guid? funcionarioId, ITenantProvider tenant, ProntagDbContext db) =>
        {
            produto.TenantId = tenant.TenantId;
            db.Produtos.Add(produto);

            if (funcionarioId.HasValue)
            {
                db.AuditoriasAlteracao.Add(new AuditoriaAlteracao
                {
                    TenantId = tenant.TenantId,
                    Entidade = "Produto",
                    Acao = "Criar",
                    EntidadeId = produto.Id,
                    FuncionarioId = funcionarioId.Value
                });
            }

            await db.SaveChangesAsync();
            return Results.Created($"/produtos/{produto.Id}", produto);
        });

        grupo.MapPut("/{id:guid}", async (Guid id, Produto atualizacao, Guid? funcionarioId, ITenantProvider tenant, ProntagDbContext db) =>
        {
            var produto = await db.Produtos.FindAsync(id);
            if (produto is null) return Results.NotFound();

            produto.Nome = atualizacao.Nome;
            produto.DiasValidade = atualizacao.DiasValidade;
            produto.Preco = atualizacao.Preco;
            produto.Icone = atualizacao.Icone;

            if (funcionarioId.HasValue)
            {
                db.AuditoriasAlteracao.Add(new AuditoriaAlteracao
                {
                    TenantId = tenant.TenantId,
                    Entidade = "Produto",
                    Acao = "Editar",
                    EntidadeId = produto.Id,
                    FuncionarioId = funcionarioId.Value
                });
            }

            await db.SaveChangesAsync();
            return Results.Ok(produto);
        });

        grupo.MapDelete("/{id:guid}", async (Guid id, Guid? funcionarioId, ITenantProvider tenant, ProntagDbContext db) =>
        {
            var produto = await db.Produtos.FindAsync(id);
            if (produto is null) return Results.NotFound();

            produto.Ativo = false;

            if (funcionarioId.HasValue)
            {
                db.AuditoriasAlteracao.Add(new AuditoriaAlteracao
                {
                    TenantId = tenant.TenantId,
                    Entidade = "Produto",
                    Acao = "Remover",
                    EntidadeId = produto.Id,
                    FuncionarioId = funcionarioId.Value
                });
            }

            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
