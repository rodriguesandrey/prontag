using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prontag.Domain;
using Prontag.Infrastructure;
using System.Text.RegularExpressions;

namespace Prontag.Api.Endpoints;

public record CriarUsuarioRequest(string Nome, string Login, string Senha, string Papel);

public static class UsuarioEndpoints
{
    private static readonly PasswordHasher<Usuario> Hasher = new();
    private static readonly Regex RegraSenha = new(@"^(?=(?:.*\d){4,})(?=.*[A-Za-z]).+$");

    public static void MapUsuarioEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/usuarios");

        grupo.MapGet("/", async (ProntagDbContext db) =>
            await db.Usuarios
                .Where(u => u.Ativo)
                .Select(u => new { u.Id, u.Nome, u.Login, u.Papel })
                .ToListAsync());

        grupo.MapPost("/", async (CriarUsuarioRequest req, ITenantProvider tenant, ProntagDbContext db) =>
        {
            if (!RegraSenha.IsMatch(req.Senha))
                return Results.BadRequest("Senha precisa ter pelo menos 4 números e 1 letra.");

            if (await db.Usuarios.AnyAsync(u => u.Login == req.Login))
                return Results.BadRequest("Login já existe.");

            var usuario = new Usuario
            {
                TenantId = tenant.TenantId,
                Nome = req.Nome,
                Login = req.Login,
                Papel = req.Papel,
                SenhaHash = ""
            };
            usuario.SenhaHash = Hasher.HashPassword(usuario, req.Senha);

            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();

            return Results.Created($"/usuarios/{usuario.Id}", new { usuario.Id, usuario.Nome, usuario.Login, usuario.Papel });
        });

        grupo.MapDelete("/{id:guid}", async (Guid id, ProntagDbContext db) =>
        {
            var usuario = await db.Usuarios.FindAsync(id);
            if (usuario is null) return Results.NotFound();
            usuario.Ativo = false;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        grupo.MapGet("/", async (ProntagDbContext db) =>
    await db.Usuarios
        .Where(u => u.Ativo)
        .Select(u => new { u.Id, u.Nome, u.Login, u.Papel, u.PedidoRedefinicao, u.PedidoRedefinicaoEm })
        .ToListAsync());

        grupo.MapPost("/{id:guid}/redefinir-senha", async (Guid id, string novaSenha, ProntagDbContext db) =>
{
    if (!RegraSenha.IsMatch(novaSenha))
        return Results.BadRequest("Senha precisa ter pelo menos 4 números e 1 letra.");

    var usuario = await db.Usuarios.FindAsync(id);
    if (usuario is null) return Results.NotFound();

    usuario.SenhaHash = Hasher.HashPassword(usuario, novaSenha);
    usuario.PedidoRedefinicao = false;
    usuario.PedidoRedefinicaoEm = null;
    await db.SaveChangesAsync();

    return Results.NoContent();
});
    }
}
