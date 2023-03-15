namespace Sample.Api.Middleware
{

    internal class OpenIdConnectConfigurationSettings
    {
        /// <summary>
        /// The endpoint to the OpenId Connect Metadata document
        /// </summary>
        public string? OpenIdConnectUrl { get; set; }


        /// <summary>
        /// The registered application (client) id of this backend api.
        /// </summary>
        public string? Audience { get; set; }
    }
}