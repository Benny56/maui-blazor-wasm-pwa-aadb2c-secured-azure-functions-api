namespace Sample.Razor.Services
{

    public class ApiSettings
    {
        /// <summary>
        /// <see cref="HttpClient"/> instances that apply the provided configuration can be retrieved using
        /// <see cref="IHttpClientFactory.CreateClient(string)"/> and providing the matching name.
        /// The default name is "ApiClient". 
        /// </summary>
        public string HttpClientName { get; set; } = "ApiClient";


        /// <summary>
        /// The base address of the API endpoint to which the access token will be attached, for example: https://api.contoso.com
        /// </summary>
        public string? BaseAddress { get; set; }


        /// <summary>
        /// The list of scopes to use when requesting an access token
        /// </summary>
        public IEnumerable<string> Scopes { get; set; } = new List<string>();

    }
}