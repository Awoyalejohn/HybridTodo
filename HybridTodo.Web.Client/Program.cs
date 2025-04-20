using HybridTodo.Shared.Abstractions.Clients;
using HybridTodo.Shared.Services;
using HybridTodo.Web.Client.Clients;
using HybridTodo.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using HybridTodo.Shared.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddHttpClient<IAuthClient, AuthClient>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

    // The cookie auth stack detects this header and avoids redirects for unauthenticated requests
    client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
});

builder.Services.AddHttpClient<ITodoClient, TodoClient>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

    // The cookie auth stack detects this header and avoids redirects for unauthenticated requests
    client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
});

builder.Services.AddHttpClient<IUserClient, UserClient>(client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

    // The cookie auth stack detects this header and avoids redirects for unauthenticated requests
    client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
});

builder.Services.AddFluentUIComponents();

// Add device-specific services used by the HybridTodo.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
//builder.Services.AddScoped<IAuthClient, AuthClient>();

await builder.Build().RunAsync();
