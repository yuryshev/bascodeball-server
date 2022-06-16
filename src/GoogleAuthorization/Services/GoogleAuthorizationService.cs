using GoogleAuthorization.Dtos;
using GoogleAuthorization.Options;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;

namespace GoogleAuthorization.Services
{
    public class GoogleAuthorizationService
    {
        private readonly IConfiguration _configuration;
        private readonly GoogleOAuthOptions _googleOauthOptions;

        private readonly IHttpClientFactory _httpClientFactory;

        public GoogleAuthorizationService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _googleOauthOptions = new GoogleOAuthOptions();
            _configuration.GetSection("GoogleOAuth").Bind(_googleOauthOptions);

            _httpClientFactory = httpClientFactory;
        }

        public string GetAuthorizationCodeEndpoint()
        {

            var queryParams = new Dictionary<string, string>
            {
                {"client_id", _googleOauthOptions.ClientId },
                {"response_type", "code" },
                { "scope", _googleOauthOptions.Scope},
                { "redirect_uri", _googleOauthOptions.RedirectEndpoint},
                { "state", _googleOauthOptions.State}
            };

            var url = QueryHelpers.AddQueryString(_googleOauthOptions.ServerEndpoint, queryParams!);

            return url;
        }

        public async Task<AccessTokenPayloadDto> GetAuthorizationToken(string code)
        {
            var bodyParams = new Dictionary<string, string>
            {
                { "code", code},
                { "client_id", _googleOauthOptions.ClientId},
                { "client_secret", _googleOauthOptions.ClientSecret},
                { "redirect_uri", _googleOauthOptions.RedirectEndpoint},
                { "grant_type", "authorization_code"}

            };

            var request = new HttpRequestMessage(HttpMethod.Post, _googleOauthOptions.TokenEndpoint);
            var httpContent = new FormUrlEncodedContent(bodyParams);

            request.Content = httpContent;

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);

            var jsonString = await response.Content.ReadAsStringAsync();

            AccessTokenDto? token =
                JsonSerializer.Deserialize<AccessTokenDto>(jsonString);

            string payloadString = token!.Token.Split('.')[1];

            string payloadJson = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(payloadString));

            var accessTokenPayload = JsonSerializer.Deserialize<AccessTokenPayloadDto>(payloadJson);


            return accessTokenPayload!;
        }

        public async Task<string> ProvideJWTUrl(AccessTokenPayloadDto accessTokenPayload)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var queryParams = new Dictionary<string, string>
            {
                { "email", accessTokenPayload.Email }
            };

            var url = QueryHelpers.AddQueryString("https://localhost:5001/token", queryParams!);

            var response = await httpClient.GetAsync(url);

            var jsonString = await response.Content.ReadAsStringAsync();

            var getJwtErrorDto = JsonSerializer.Deserialize<GetJwtErrorDto>(jsonString);
            var getJwtDto = JsonSerializer.Deserialize<GetJwtDto>(jsonString);

            if (getJwtErrorDto == null)
            {
                Console.WriteLine("null");
            }

            if (getJwtErrorDto.ErrorText == "Invalid email.")
            {
                return $"http://localhost:3000/registration?email={accessTokenPayload!.Email}&picture={accessTokenPayload!.Picture}";
            }
            else
            {
                return $"http://localhost:3000?access_token={getJwtDto!.Token}";
            }
        } 
    }
}



