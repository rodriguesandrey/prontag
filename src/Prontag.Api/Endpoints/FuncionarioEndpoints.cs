using Microsoft.EntityFrameworkCore;
using Prontag.Domain;
using Prontag.Infrastructure;

namespace Prontag.Api.Endpoints;

public static class FuncionarioEndpoints
{
    public static void MapFuncionarioEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/funcionarios");

        grupo.MapGet("/", async (ProntagDbContext db) =>
            await db.Funcionarios.Where(f => f.Ativo).ToListAsync());

        grupo.MapPost("/", async (Funcionario funcionario, Guid? funcionarioId, ITenantProvider tenant, ProntagDbContext db) =>
        {
            funcionario.TenantId = tenant.TenantId;
            db.Funcionarios.Add(funcionario);

            if (funcionarioId.HasValue)
            {
                db.AuditoriasAlteracao.Add(new AuditoriaAlteracao
                {
                    TenantId = tenant.TenantId,
                    Entidade = "Funcionario",
                    Acao = "Criar",
                    EntidadeId = funcionario.Id,
                    FuncionarioId = funcionarioId.Value
                });
            }

            await db.SaveChangesAsync();
            return Results.Created($"/funcionarios/{funcionario.Id}", funcionario);
        });

        grupo.MapPut("/{id:guid}", async (Guid id, Funcionario atualizacao, Guid? funcionarioId, ITenantProvider tenant, ProntagDbContext db) =>
        {
            var funcionario = await db.Funcionarios.FindAsync(id);
            if (funcionario is null) return Results.NotFound();

            funcionario.Nome = atualizacao.Nome;

            if (funcionarioId.HasValue)
            {
                db.AuditoriasAlteracao.Add(new AuditoriaAlteracao
                {
                    TenantId = tenant.TenantId,
                    Entidade = "Funcionario",
                    Acao = "Editar",
                    EntidadeId = funcionario.Id,
                    FuncionarioId = funcionarioId.Value
                });
            }

            await db.SaveChangesAsync();
            return Results.Ok(funcionario);
        });

        grupo.MapDelete("/{id:guid}", async (Guid id, Guid? funcionarioId, ITenantProvider tenant, ProntagDbContext db) =>
        {
            var funcionario = await db.Funcionarios.FindAsync(id);
            if (funcionario is null) return Results.NotFound();

            funcionario.Ativo = false;

            if (funcionarioId.HasValue)
            {
                db.AuditoriasAlteracao.Add(new AuditoriaAlteracao
                {
                    TenantId = tenant.TenantId,
                    Entidade = "Funcionario",
                    Acao = "Remover",
                    EntidadeId = funcionario.Id,
                    FuncionarioId = funcionarioId.Value
                });
            }

            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
