using System.Text.Json.Serialization;

namespace GoogleAuthorization.API.Dtos
{
    public class AccessTokenDto
    {
        [JsonPropertyName("id_token")]
        public string Token { get; set; } = null!;
    }
}
