using Common.DbModels;

namespace Lobby.Models
{
    public class Group
    {
        public string Id { get; set; }

        public List<Team> Teams { get; set; } = new List<Team>();

        public Exercise? Task { get; set; }
    }
}
