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
    }
}