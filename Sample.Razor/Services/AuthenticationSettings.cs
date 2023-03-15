namespace Sample.Razor.Services
{

    public class AuthenticationSettings
    {
        /// <summary>
        /// The authority which issues identity and/or access tokens
        /// </summary>
        public string? Authority { get; set; }

        /// <summary>
        /// The id of the registered application
        /// </summary>
        public string? ClientId { get; set; }


        /// <summary>
        /// Indicates whether the authority should be validated or not
        /// </summary>
        public bool ValidateAuthority { get; set; }


        /// <summary>
        /// <see cref="HttpClient"/> instances that apply the provided configuration can be retrieved using
        /// <see cref="IHttpClientFactory.CreateClient(string)"/> and providing the matching name.
        /// The default name is "ApiClient". 
        /// </summary>
        public string ApiClientName { get; set; } = "ApiClient";


        /// <summary>
        /// The base address of the API endpoint to which the access token will be attached, for example: https://api.contoso.com
        /// </summary>
        public string? ApiBaseAddress { get; set; }


        /// <summary>
        /// The list of scopes to use when requesting an access token
        /// </summary>
        public IEnumerable<string> Scopes { get; set; } = new List<string>();

    }
}