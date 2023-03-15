using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sample.Razor.Extensions;
using Sample.Razor.Services;
using Sample.Web;
using Sample.Web.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// Add an API client configuration to the HttpClientFactory
builder.Services
    .Configure<AuthenticationSettings>(settings => builder.Configuration.Bind("Authentication", settings))
    .AddScoped<ApiAuthorizationMessageHandler>()
    .AddApiClient()
    .AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

// Add authentication against Azure Active directory B2C
builder.Services
    .AddMsalAuthentication(options =>
    {
        builder.Configuration.Bind("Authentication", options.ProviderOptions.Authentication);
        builder.Configuration.Bind("Authentication:Scopes", options.ProviderOptions.DefaultAccessTokenScopes);
        options.ProviderOptions.Cache.CacheLocation = "localStorage";
    });

await builder
    .Build()
    .RunAsync();

