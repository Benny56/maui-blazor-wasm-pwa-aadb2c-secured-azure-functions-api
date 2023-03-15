using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sample.Razor.Services;


namespace Sample.Razor.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddApiClient(this IServiceCollection services)
        {
            var options = services.BuildServiceProvider().GetService<IOptions<AuthenticationSettings>>();
            ArgumentNullException.ThrowIfNull(options);

            var settings = options.Value;
            var uri = string.IsNullOrEmpty(settings.ApiBaseAddress) ? null : new Uri(settings.ApiBaseAddress);

            // We add an HttpClient to the IHttpClientFactory first, in case more HttpClients are needed 
            var builder = services.AddHttpClient(settings.ApiClientName, client => client.BaseAddress = uri);

            // Let's create the main HttpClient from the factory
            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(settings.ApiClientName));
            return builder;
        }
    }
}