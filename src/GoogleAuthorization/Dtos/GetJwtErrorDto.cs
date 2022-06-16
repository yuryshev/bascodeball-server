using System.Text.Json.Serialization;

namespace GoogleAuthorization.Dtos
{
    public class GetJwtErrorDto
    {
        [JsonPropertyName("errorText")]
        public string ErrorText { get; set; } = null!;
    }
}
