using System.Text.Json.Serialization;

namespace GoogleAuthorization.API.Dtos
{
    public class AccessTokenPayloadDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("picture")]
        public string Picture { get; set; } = null!;

        [JsonPropertyName("family_name")]
        public string LastName { get; set; } = null!;

        [JsonPropertyName("given_name")]
        public string FirstName { get; set; } = null!;
    }
}
