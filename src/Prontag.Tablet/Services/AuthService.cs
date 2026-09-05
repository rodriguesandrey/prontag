using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Prontag.Tablet.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public string? Token { get; private set; }
    public string? Nome { get; private set; }
    public string? Papel { get; private set; }
    public bool Autenticado => Token is not null;

    public AuthService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task InicializarAsync()
    {
        var module = await _js.InvokeAsync<IJSObjectReference>("import", "./js/localStorage.js");
        var tokenSalvo = await module.InvokeAsync<string?>("obter", "prontag_token");

        if (string.IsNullOrEmpty(tokenSalvo)) return;

        var resposta = await _http.GetAsync($"auth/validar?token={tokenSalvo}");
        if (resposta.IsSuccessStatusCode)
        {
            var dados = await resposta.Content.ReadFromJsonAsync<ValidarResposta>();
            Token = tokenSalvo;
            Nome = dados?.Nome;
            Papel = dados?.Papel;
        }
        else
        {
            await module.InvokeVoidAsync("remover", "prontag_token");
        }
    }

    public async Task<bool> LoginAsync(string login, string senha)
    {
        var resposta = await _http.PostAsJsonAsync("auth/login", new { Login = login, Senha = senha });
        if (!resposta.IsSuccessStatusCode) return false;

        var dados = await resposta.Content.ReadFromJsonAsync<LoginResposta>();
        if (dados is null) return false;

        Token = dados.Token.ToString();
        Nome = dados.Nome;
        Papel = dados.Papel;

        var module = await _js.InvokeAsync<IJSObjectReference>("import", "./js/localStorage.js");
        await module.InvokeVoidAsync("definir", "prontag_token", Token);

        return true;
    }

    public async Task LogoutAsync()
    {
        Token = null;
        Nome = null;
        Papel = null;
        var module = await _js.InvokeAsync<IJSObjectReference>("import", "./js/localStorage.js");
        await module.InvokeVoidAsync("remover", "prontag_token");
    }

    private class LoginResposta
    {
        public Guid Token { get; set; }
        public string Nome { get; set; } = "";
        public string Papel { get; set; } = "";
    }

    private class ValidarResposta
    {
        public string Nome { get; set; } = "";
        public string Papel { get; set; } = "";
    }
}
