using Common.DbModels;

namespace Lobby.Models
{
    public class Team
    {
        public string Id { get; set; }

        public List<User> Players { get; set; } = new List<User>();
    }
}
