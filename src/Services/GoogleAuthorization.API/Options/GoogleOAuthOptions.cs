namespace GoogleAuthorization.API.Options
{
    public class GoogleOAuthOptions
    {
        public string ClientId { get; set; } = null!;

        public string ClientSecret { get; set; } = null!;

        public string ServerEndpoint { get; set; } = null!;

        public string TokenEndpoint { get; set; } = null!;

        public string RedirectEndpoint { get; set; } = null!;

        public string Scope { get; set; } = null!;

        public string State { get; set; } = null!;
    }
}
