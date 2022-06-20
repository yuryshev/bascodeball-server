using System.Linq;
using Common.DbModels;
using Lobby.Models;

namespace Lobby.Services
{
    public class GroupService
    {
        public List<Group> Groups = new List<Group>();

        private const int TeamMembersCount = 2;
        private const int GroupTeamsCount = 2;

        public bool IsFull(string groupId)
        {
           return Groups.FirstOrDefault(g => g.Id == groupId).Teams.TrueForAll(t => t.Players.Count == TeamMembersCount);
        }

        public Group AddPlayer(User? inputPlayer)
        {
            // Reconect Player
            foreach (var group in Groups)
            {
                foreach (var team in group.Teams)
                {
                    foreach (var player in team.Players)
                    {
                        if (player.UserId == inputPlayer.UserId && player.ConnectionId != inputPlayer.ConnectionId)
                        {
                            player.ConnectionId = inputPlayer.ConnectionId;
                            return group;
                        }
                    }
                }
            }

            // Create Group
            if (Groups.Count == 0 || (Groups.Last().Teams.Count == GroupTeamsCount && Groups.Last().Teams.Last().Players.Count == TeamMembersCount))
            {
                Groups.Add(
                    new Group
                    {
                        Id = Guid.NewGuid().ToString(),
                        Teams = new List<Team>
                        {
                            new() { Id = Guid.NewGuid().ToString() },
                            new() { Id = Guid.NewGuid().ToString() },
                        }
                    });
            }
            
            //Add Player
            var lastGroup = Groups.Last(g => g.Teams.Any(t => t.Players.Count != TeamMembersCount));
            lastGroup.Teams.First(t => t.Players.Count != TeamMembersCount).Players.Add(inputPlayer);

            return lastGroup;
        }
    }
}
