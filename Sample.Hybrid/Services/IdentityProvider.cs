using Microsoft.Identity.Client;
using Sample.Razor.Services;


namespace Sample.Hybrid.Services
{
    public static class IdentityProvider
    {


        public static IPublicClientApplication Create(AuthenticationSettings settings)

#if ANDROID
        => PublicClientApplicationBuilder
            .Create(settings.ClientId)
            .WithB2CAuthority(settings.Authority)
            .WithRedirectUri($"msal{settings.ClientId}://auth")
            .WithParentActivityOrWindow(() => Platform.CurrentActivity)
            .Build();
#elif IOS
        => PublicClientApplicationBuilder
            .Create(settings.ClientId)
            .WithB2CAuthority(settings.Authority)
            .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
            .WithRedirectUri($"msal{settings.ClientId}://auth")
            .Build();
#else
            => PublicClientApplicationBuilder
                .Create(settings.ClientId)
                .WithB2CAuthority(settings.Authority)
                .WithRedirectUri("http://localhost")
                .Build();
#endif

    }
}