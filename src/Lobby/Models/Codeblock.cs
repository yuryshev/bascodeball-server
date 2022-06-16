using System.Text.Json.Serialization;

namespace Lobby.Models
{
    public class Codeblock
    {
        public string Id { get; set; } = null!;

        public string ClientId { get; set; } = null!;

        public string ConnectionId { get; set; } = null!;

        public string GroupId { get; set; } = null!;

        public string Code { get; set; } = null!;
    }
}
