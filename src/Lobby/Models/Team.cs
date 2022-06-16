namespace Lobby.Models
{
    public class Team
    {
        public string Id { get; set; } = null!;

        public List<Player> Players { get; set; } = new List<Player> { };
    }
}
