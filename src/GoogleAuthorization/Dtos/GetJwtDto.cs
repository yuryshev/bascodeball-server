using System.Text.Json.Serialization;

namespace GoogleAuthorization.Dtos
{
    public class GetJwtDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("access_token")]
        public string Token { get; set; } = null!;
    }
}
