namespace Lobby.Models
{
    public class Player
    {
        public string Id { get; set; } = null!;

        public string ConnectionId { get; set; } = null!;

        public string Nickname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Picture { get; set; } = null!;

        public int Rating { get; set; }
    }
}
