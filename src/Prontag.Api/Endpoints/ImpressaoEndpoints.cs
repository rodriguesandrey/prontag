using Microsoft.EntityFrameworkCore;
using Prontag.Domain;
using Prontag.Infrastructure;

namespace Prontag.Api.Endpoints;

public record CriarImpressaoRequest(Guid ProdutoId, Guid? FuncionarioId, TipoEtiqueta Tipo);

public static class ImpressaoEndpoints
{
    public static void MapImpressaoEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/impressoes");

        grupo.MapPost("/", async (CriarImpressaoRequest request, ITenantProvider tenant, ProntagDbContext db) =>
        {
            var produto = await db.Produtos.FindAsync(request.ProdutoId);
            if (produto is null)
                return Results.NotFound("Produto não encontrado.");

            if (request.Tipo == TipoEtiqueta.Produto && request.FuncionarioId is null)
                return Results.BadRequest("Etiqueta de Produto exige um responsável.");

            var fabricacao = DateOnly.FromDateTime(DateTime.UtcNow);
            var vencimento = fabricacao.AddDays(produto.DiasValidade);

            var impressao = new Impressao
            {
                TenantId = tenant.TenantId,
                Tipo = request.Tipo,
                ProdutoId = produto.Id,
                FuncionarioId = request.Tipo == TipoEtiqueta.Produto ? request.FuncionarioId : null,
                Fabricacao = fabricacao,
                Vencimento = vencimento
            };

            db.Impressoes.Add(impressao);
            await db.SaveChangesAsync();

            return Results.Created($"/impressoes/{impressao.Id}", new
            {
                impressao.Id,
                Produto = produto.Nome,
                produto.Preco,
                impressao.Fabricacao,
                impressao.Vencimento,
                impressao.FuncionarioId
            });
        });

        grupo.MapGet("/", async (ProntagDbContext db) =>
            await db.Impressoes
                .Include(i => i.Produto)
                .Include(i => i.Funcionario)
                .OrderByDescending(i => i.CriadoEm)
                .ToListAsync());
    }
}