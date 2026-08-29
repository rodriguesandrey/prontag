using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Prontag.Tablet;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://organic-xylophone-wpjqgpw5vvgfgqqx-5152.app.github.dev/")
});

builder.Services.AddScoped<Prontag.Tablet.Services.ImpressoraService>();
await builder.Build().RunAsync();