using Microsoft.EntityFrameworkCore;
using Prontag.Domain;
using Prontag.Infrastructure;

namespace Prontag.Api.Endpoints;

public static class RelatorioEndpoints
{
    public static void MapRelatorioEndpoints(this WebApplication app)
    {
        app.MapGet("/relatorios", async (DateOnly inicio, DateOnly fim, ProntagDbContext db) =>
        {
            var dataInicio = inicio.ToDateTime(TimeOnly.MinValue);
            var dataFim = fim.ToDateTime(TimeOnly.MaxValue);

            var impressoes = await db.Impressoes
                .Include(i => i.Funcionario)
                .Where(i => i.CriadoEm >= dataInicio && i.CriadoEm <= dataFim)
                .ToListAsync();

            var agrupado = impressoes
                .GroupBy(i => i.FuncionarioId ?? Guid.Empty)
                .Select(g => new
                {
                    Funcionario = g.First().Funcionario?.Nome ?? "Sem responsável (Expositor)",
                    EtiquetasProduto = g.Count(i => i.Tipo == TipoEtiqueta.Produto),
                    EtiquetasExpositor = g.Count(i => i.Tipo == TipoEtiqueta.Expositor),
                    Total = g.Count()
                })
                .OrderByDescending(r => r.Total)
                .ToList();

            return Results.Ok(agrupado);
        });
    }
}
