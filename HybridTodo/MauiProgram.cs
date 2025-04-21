using HybridTodo.Abstractions.Services;
using HybridTodo.Clients;
using HybridTodo.Services;
using HybridTodo.Shared.Abstractions.Clients;
using HybridTodo.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Reflection;

namespace HybridTodo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HybridTodo.appsettings.json");
            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();
            builder.Configuration.AddConfiguration(config);

            builder.Services.AddAuthorizationCore();
            builder.Services.AddCascadingAuthenticationState();


            // Configure data protection, setup the application discriminator so that the data protection keys can be shared between the BFF and this API
            builder.Services.AddDataProtection(o => o.ApplicationDiscriminator = "HybridTodo");

            // Add device-specific services used by the HybridTodo.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddScoped<IComponentStateManager, MauiComponentStateManager>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["HybridTodoApiUrl"] ?? throw new ArgumentNullException("HybridTodoApiUrl")) });

            builder.Services.AddHttpClient<IAuthClient, AuthClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["HybridTodoApiUrl"] ?? throw new ArgumentNullException("HybridTodoApiUrl"));
            });

            builder.Services.AddScoped<TokenAppendingHandler>();
            builder.Services.AddHttpClient<ITodoClient, Shared.Clients.TodoClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["HybridTodoApiUrl"] ?? throw new ArgumentNullException("HybridTodoApiUrl"));

                // The cookie auth stack detects this header and avoids redirects for unauthenticated requests
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
            })
            .AddHttpMessageHandler<TokenAppendingHandler>();

            builder.Services.AddHttpClient<IUserClient, Shared.Clients.UserClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["HybridTodoApiUrl"] ?? throw new ArgumentNullException("HybridTodoApiUrl"));

                // The cookie auth stack detects this header and avoids redirects for unauthenticated requests
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
            })
            .AddHttpMessageHandler<TokenAppendingHandler>();

            builder.Services.AddScoped<ITokenStorageService, TokenStorageService>();
            // This is our custom provider
            builder.Services.AddScoped<AuthenticationStateProvider, MauiAuthenticationStateProvider>();

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddFluentUIComponents();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
