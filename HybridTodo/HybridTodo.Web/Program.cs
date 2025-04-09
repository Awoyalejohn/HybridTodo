using HybridTodo.Shared.Clients;
using HybridTodo.Shared.Services;
using HybridTodo.Web;
using HybridTodo.Web.Clients;
using HybridTodo.Web.Components;
using HybridTodo.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();
builder.Services.AddFluentUIComponents();

builder.Services.AddCascadingAuthenticationState();

// Add device-specific services used by the HybridTodo.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
//builder.Services.AddScoped<IAuthClient, AuthClient>();
//builder.Services.AddHttpClient<IAuthClient, AuthClient>();
builder.Services.AddKeyedScoped<IAuthClient, AuthClient>("Web.Server");
builder.Services.AddKeyedScoped<IAuthClient, HybridTodo.Web.Client.Clients.AuthClient>("Web.Client");

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();


// Configure the HttpClient for the backend API
//builder.Services.AddHttpClient<AuthClient>(client =>
//{
//    client.BaseAddress = new("https://localhost:7175");
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(HybridTodo.Shared._Imports).Assembly,
        typeof(HybridTodo.Web.Client._Imports).Assembly);


app.MapAuthEndpoints();

app.Run();
