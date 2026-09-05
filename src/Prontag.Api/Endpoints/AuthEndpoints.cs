using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prontag.Domain;
using Prontag.Infrastructure;

namespace Prontag.Api.Endpoints;

public record LoginRequest(string Login, string Senha);

public static class AuthEndpoints
{
    private static readonly PasswordHasher<Usuario> Hasher = new();

    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/auth/login", async (LoginRequest req, ProntagDbContext db) =>
        {
            var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Login == req.Login && u.Ativo);
            if (usuario is null) return Results.Unauthorized();

            var resultado = Hasher.VerifyHashedPassword(usuario, usuario.SenhaHash, req.Senha);
            if (resultado == PasswordVerificationResult.Failed) return Results.Unauthorized();

            usuario.Token = Guid.NewGuid();
            usuario.TokenExpiraEm = DateTime.UtcNow.AddHours(12);
            await db.SaveChangesAsync();

            return Results.Ok(new { Token = usuario.Token, Nome = usuario.Nome, Papel = usuario.Papel });
        });

        app.MapGet("/auth/validar", async (Guid token, ProntagDbContext db) =>
        {
            var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Token == token && u.Ativo);
            if (usuario is null || usuario.TokenExpiraEm < DateTime.UtcNow) return Results.Unauthorized();

            return Results.Ok(new { Nome = usuario.Nome, Papel = usuario.Papel });
        });
    }
}
