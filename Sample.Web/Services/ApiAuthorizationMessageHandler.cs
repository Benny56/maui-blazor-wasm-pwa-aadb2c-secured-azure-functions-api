using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using Sample.Razor.Services;


namespace Sample.Web.Services
{
    /// <summary>
    /// An implementation of a <see cref="DelegatingHandler"/> that attaches access tokens to outgoing <see cref="HttpResponseMessage"/> instances.
    /// Access tokens will only be added when the request URI is within one of the base addresses configured using
    /// <see cref="ConfigureHandler(IEnumerable{string}, IEnumerable{string}, string)"/>.
    /// </summary>
    public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public ApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation, IOptions<AuthenticationSettings> options) : base(provider, navigation)
        {
            var settings = options.Value;
            ArgumentNullException.ThrowIfNull(settings, nameof(settings));

            ConfigureHandler(
                authorizedUrls: new[] { settings.ApiBaseAddress },
                scopes: settings.Scopes);
        }
    }
}