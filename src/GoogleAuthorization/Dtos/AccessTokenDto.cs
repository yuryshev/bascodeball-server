using System.Text.Json.Serialization;

namespace GoogleAuthorization.Dtos
{
    public class AccessTokenDto
    {
        [JsonPropertyName("id_token")]
        public string Token { get; set; } = null!;
    }
}
