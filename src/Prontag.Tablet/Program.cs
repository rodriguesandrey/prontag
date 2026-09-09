using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Prontag.Tablet;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://prontag-api.onrender.com")
});

builder.Services.AddScoped<Prontag.Tablet.Services.ImpressoraService>();
builder.Services.AddScoped<Prontag.Tablet.Services.AuthService>();
await builder.Build().RunAsync();