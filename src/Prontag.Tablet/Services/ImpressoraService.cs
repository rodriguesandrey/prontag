using Microsoft.JSInterop;

namespace Prontag.Tablet.Services;

public class ImpressoraService : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public ImpressoraService(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./js/webUsbPrinter.js").AsTask());
    }

    public async Task<string> ConectarAsync()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<string>("conectar");
    }

    public async Task<string?> ReconectarSilenciosamenteAsync()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<string?>("reconectarSilenciosamente");
    }

    public async Task ImprimirAsync(string zpl)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("imprimir", zpl);
    }

    public async Task<bool> EstaConectadoAsync()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("estaConectado");
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
