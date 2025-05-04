using HybridTodo.Api;
using HybridTodo.Api.Data;
using HybridTodo.Api.Endpoints;
using HybridTodo.Api.Extensions;
using HybridTodo.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => options.AddBearerTokenAuthentication());

// Configure data protection, setup the application discriminator
// so that the data protection keys can be shared between the BFF and this API
builder.Services.AddDataProtection(o => o.ApplicationDiscriminator = "HybridTodo");

builder.Services.AddAuthentication()
    .AddBearerToken(BearerTokenDefaults.AuthenticationScheme, o =>
    {
        var serviceProvider = builder.Services.BuildServiceProvider();
        var dataProtectionProvider = serviceProvider.GetRequiredService<IDataProtectionProvider>();

        // Purpose chain must match exactly between apps
        var bearerProtector = dataProtectionProvider.CreateProtector("HybridTodo", BearerTokenDefaults.AuthenticationScheme);
        var refreshProtector = dataProtectionProvider.CreateProtector("HybridTodo", AuthConstants.RefreshToken);

        o.BearerTokenProtector = new TicketDataFormat(bearerProtector);
        o.RefreshTokenProtector = new TicketDataFormat(refreshProtector);
    });
builder.Services.AddAuthorization();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var connectionString = builder.Configuration.GetConnectionString("HybridTodoConnection");
ArgumentNullException.ThrowIfNull(connectionString);

builder.Services.AddDbContext<HybridTodoContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Servers = [];
        options.Authentication = new() { PreferredSecurityScheme = BearerTokenDefaults.AuthenticationScheme };
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandler();
app.UseAuthorization();

app.Map("/", () => Results.Redirect("/scalar/v1"));

app.MapAuthEndpoints();
app.MapTestAuthEndpoints();
app.MapUserEndpoints();
app.MapTodoEndpoints();

app.Run();
