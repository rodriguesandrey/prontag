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

        grupo.MapPost("/", async (Funcionario funcionario, ITenantProvider tenant, ProntagDbContext db) =>
        {
            funcionario.TenantId = tenant.TenantId;
            db.Funcionarios.Add(funcionario);
            await db.SaveChangesAsync();
            return Results.Created($"/funcionarios/{funcionario.Id}", funcionario);
        });
        grupo.MapPut("/{id:guid}", async (Guid id, Funcionario atualizacao, ProntagDbContext db) =>
{
    var funcionario = await db.Funcionarios.FindAsync(id);
    if (funcionario is null) return Results.NotFound();

    funcionario.Nome = atualizacao.Nome;

    await db.SaveChangesAsync();
    return Results.Ok(funcionario);
});

grupo.MapDelete("/{id:guid}", async (Guid id, ProntagDbContext db) =>
{
    var funcionario = await db.Funcionarios.FindAsync(id);
    if (funcionario is null) return Results.NotFound();

    funcionario.Ativo = false;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
    }
}