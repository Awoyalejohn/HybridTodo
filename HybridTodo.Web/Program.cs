using HybridTodo.Shared.Abstractions.Clients;
using HybridTodo.Shared.Services;
using HybridTodo.Web;
using HybridTodo.Web.Clients;
using HybridTodo.Web.Components;
using HybridTodo.Web.Endpoints;
using HybridTodo.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();


builder.Services.AddCascadingAuthenticationState();

// Configure data protection, setup the application discriminator so that the data protection keys can be shared between the BFF and this API
builder.Services.AddDataProtection(o => o.ApplicationDiscriminator = "HybridTodo");

// Add device-specific services used by the HybridTodo.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddHttpClient<IAuthClient, AuthClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["HybridTodoApiUrl"] ?? throw new ArgumentNullException("HybridTodoApiUrl"));
});

builder.Services.AddScoped<CookieAppendingHandler>();
builder.Services.AddHttpClient<ITodoClient, HybridTodo.Shared.Clients.TodoClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseAddress"] ?? throw new ArgumentNullException("BaseAddress"));

    // The cookie auth stack detects this header and avoids redirects for unauthenticated requests
    client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
})
.AddHttpMessageHandler<CookieAppendingHandler>();

// Add the forwarder to make sending requests to the backend easier
builder.Services.AddHttpForwarder();
//builder.Services.AddHttpForwarderWithServiceDiscovery();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddFluentUIComponents();


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
app.MapTodoEndpoints();

app.Run();
