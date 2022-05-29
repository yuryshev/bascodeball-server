using System.Text.Json.Serialization;

namespace Client.Models
{
    public class User
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("loginName")]
        public string LoginName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

    }
}
