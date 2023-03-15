using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Sample.Razor.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;


namespace Sample.Hybrid.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        /*************/
        /* Constants */
        /*************/

        private const string TOKEN_STORAGEKEY = "access_token";
        private const string AUTHENTICATION_TYPE = "AzureAdB2C authentication";

        /**********/
        /* Fields */
        /**********/

        private readonly IPublicClientApplication _identityClient;
        private readonly HttpClient _apiClient;

        /**************/
        /* Properties */
        /**************/

        public IEnumerable<string> Scopes { get; }

        /****************/
        /* Constructors */
        /****************/


        public CustomAuthenticationStateProvider(IServiceProvider serviceProvider)
            : this(serviceProvider.GetRequiredService<IOptions<AuthenticationSettings>>(), serviceProvider.GetRequiredService<HttpClient>()) { }


        public CustomAuthenticationStateProvider(IOptions<AuthenticationSettings> options, HttpClient apiClient)
            : this(options.Value, apiClient) { }


        public CustomAuthenticationStateProvider(AuthenticationSettings settings, HttpClient apiClient)
        {
            _identityClient = IdentityProvider.Create(settings);
            _apiClient = apiClient;
            Scopes = settings.Scopes;
        }

        /*********************/
        /* Protected methods */
        /*********************/

        public async Task LoginAsync()
        {
            try
            {
                // Acquire access token and save it
                var result = await _identityClient.AcquireTokenInteractive(Scopes).ExecuteAsync();

                // Save current access token
                await SecureStorage.SetAsync(TOKEN_STORAGEKEY, result.AccessToken);

                // Providing the current identity information
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public async Task LogoutAsync()
        {
            // Clear accounts
            var accounts = await _identityClient.GetAccountsAsync();
            foreach (var account in accounts)
            {
                await _identityClient.RemoveAsync(account);
            }

            // Cleare storage
            SecureStorage.Remove(TOKEN_STORAGEKEY);

            // Update identity information
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Retrieve access token from storage
            var access_token = await SecureStorage.GetAsync(TOKEN_STORAGEKEY);

            // If access token has not expired return user
            if (IsNotExpired(access_token, out JwtSecurityToken jwtSecurityToken))
            {
                SetAuthorizationHeader(access_token);
                var claims = new List<Claim>(jwtSecurityToken.Claims)
            {
                new Claim(ClaimTypes.Name, jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty)
            };
                var identity = new ClaimsIdentity(claims, AUTHENTICATION_TYPE);
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }

            // Missing or expired access token
            ClearAuthorizationHeader();
            return new AuthenticationState(new ClaimsPrincipal());
        }

        /*******************/
        /* Private methods */
        /*******************/

        private void SetAuthorizationHeader(string access_token)
            => _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);


        private void ClearAuthorizationHeader()
            => _apiClient.DefaultRequestHeaders.Authorization = null;


        private static bool IsNotExpired(string token, out JwtSecurityToken jwtSecurityToken)
        {
            if (string.IsNullOrEmpty(token))
            {
                jwtSecurityToken = null;
                return false;
            }

            jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtSecurityToken.ValidTo > DateTime.UtcNow;
        }


    }
}