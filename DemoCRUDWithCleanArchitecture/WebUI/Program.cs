using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using RestEase.HttpClientFactory;
using System.Globalization;
using WebUI;
using RestEase;


using Application.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;

builder.Services.AddRadzenComponents();


builder.Services.AddAuthorizationCore(b =>
{
    b.AddPolicy("Admin", p =>
    {
        p.RequireRole("Admin");
        p.RequireClaim("ABC", "1");
    });

});

//Register client and services use RestEase library
// Register the RestEase client
builder.Services.AddHttpClient("API")
    .ConfigureHttpClient(x => x.BaseAddress = new Uri("https://localhost:7031"))
    .UseWithRestEaseClient<IProduct>();
    //.UseWithRestEaseClient<IProduct>();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
