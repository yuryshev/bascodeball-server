namespace Lobby.Models
{
    public class Group
    {
        public string Id { get; set; } = null!;

        public List<Team> Teams { get; set; } = new List<Team> { };

        public CodeTask? Task { get; set; }
    }
}
