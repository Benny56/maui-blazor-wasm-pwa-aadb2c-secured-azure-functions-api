using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Maui.LifecycleEvents;
using Sample.Razor.Extensions;
using Sample.Razor.Services;
using Sample.Hybrid.Services;


namespace Sample.Hybrid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
#if ANDROID
            .ConfigureLifecycleEvents(events =>
            {
                events.AddAndroid(platform =>
                {
                    platform.OnActivityResult((activity, rc, result, data) =>
                    {
                        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(rc, result, data);
                    });
                });
            })
#endif
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add configuration files to access the authentication provider and the API
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true);

            builder.Services
                .AddMauiBlazorWebView();

            builder.Services
                .AddAuthorizationCore()
                .Configure<AuthenticationSettings>(settings => builder.Configuration.Bind("Authentication", settings))
                .AddScoped<AuthenticationStateProvider>(sp => new CustomAuthenticationStateProvider(sp))
                .AddApiClient();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}