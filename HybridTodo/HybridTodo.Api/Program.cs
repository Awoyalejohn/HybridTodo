using HybridTodo.Api;
using HybridTodo.Api.Endpoints;
using HybridTodo.Api.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Scalar.AspNetCore;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => options.AddBearerTokenAuthentication());

// Configure data protection, setup the application discriminator
// so that the data protection keys can be shared between the BFF and this API
builder.Services.AddDataProtection(o => o.ApplicationDiscriminator = "HybridTodo");

//builder.Services.AddAuthentication().AddBearerToken(BearerTokenDefaults.AuthenticationScheme);

builder.Services.AddAuthentication()
    .AddBearerToken(BearerTokenDefaults.AuthenticationScheme, o =>
    {
        var serviceProvider = builder.Services.BuildServiceProvider();
        var dataProtectionProvider = serviceProvider.GetRequiredService<IDataProtectionProvider>();

        // Purpose chain must match exactly between apps
        var bearerProtector = dataProtectionProvider.CreateProtector("HybridTodo", BearerTokenDefaults.AuthenticationScheme);
        var refreshProtector = dataProtectionProvider.CreateProtector("HybridTodo", "RefreshToken");

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

var app = builder.Build();

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.Map("/", () => Results.Redirect("/scalar/v1"));

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapAuthEndpoints();
app.MapTestAuthEndpoints();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
