using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;


namespace Sample.Api.Middleware
{
    public class JwtValidationMiddleware : IFunctionsWorkerMiddleware
    {
        /**********/
        /* Fields */
        /**********/

        private readonly OpenIdConnectConfigurationSettings _openIdConnectConfigurationSettings;

        /****************/
        /* Constructors */
        /****************/

        public JwtValidationMiddleware(IConfiguration configuration)
        {
            _openIdConnectConfigurationSettings = new OpenIdConnectConfigurationSettings();
            configuration.Bind("AzureAdB2C", _openIdConnectConfigurationSettings);
        }

        /***********/
        /* Methods */
        /***********/

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            // Get http request
            var request = await context.GetHttpRequestDataAsync();

            if (request == null)
                throw new ArgumentNullException("No http request data received", nameof(request));

            // If authorization header is available
            if (request.Headers.TryGetValues("Authorization", out IEnumerable<string>? authValues))
            {
                var authValue = authValues.FirstOrDefault();
                if (authValue != null && authValue.StartsWith("Bearer"))
                {
                    // Extract the access token
                    var access_token = authValue[7..];

                    // Validate access token
                    var result = await ValidateTokenAsync(access_token, _openIdConnectConfigurationSettings);

                    if (result.IsValid)
                        await next(context);    // Continue request processing
                    else
                        await errorRespond($"Access token is invalid.\n{result.Exception.Message}");
                }
                else
                    await errorRespond("Missing 'Bearer' token in 'Authorization' header");
            }
            else
                await errorRespond("Missing 'Authorization' header");

            // Send Unauthorized status
            async Task errorRespond(string message)
            {
                var response = request.CreateResponse();
                response.StatusCode = HttpStatusCode.Unauthorized;
                await response.WriteStringAsync(message);
            }
        }


        private static async Task<TokenValidationResult> ValidateTokenAsync(string access_token, OpenIdConnectConfigurationSettings settings)
        {
            // Get configuration
            var stsDiscoveryEndpoint = settings.OpenIdConnectUrl;
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());
            OpenIdConnectConfiguration config = await configManager.GetConfigurationAsync();

            TokenValidationParameters parameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireSignedTokens = true,
                ClockSkew = TimeSpan.Zero,

                ValidAudience = settings.Audience,
                IssuerSigningKeys = config.SigningKeys,
                ValidIssuer = config.Issuer
            };

            var handler = new JwtSecurityTokenHandler();
            return await handler.ValidateTokenAsync(access_token, parameters);
        }
    }
}