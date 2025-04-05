using HybridTodo.Shared.Clients;
using HybridTodo.Shared.Services;
using HybridTodo.Web.Client.Clients;
using HybridTodo.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddFluentUIComponents();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

//builder.Services.AddHttpClient<AuthClient>(client =>
//{
//    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

//    // The cookie auth stack detects this header and avoids redirects for unauthenticated
//    // requests
//    client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
//});

// Add device-specific services used by the HybridTodo.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<IAuthClient, AuthClient>();

await builder.Build().RunAsync();
