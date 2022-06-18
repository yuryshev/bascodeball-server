using Common.DbModels;
using Lobby.Models;

namespace Lobby.Services
{
    public class GroupService
    {
        public List<Group> Groups = new List<Group>();

        public bool IsFull(string groupId)
        {
            var group = Groups.FirstOrDefault(_ => _.Id == groupId);

            int counter = 0;

            foreach(var itemTeam in group.Teams)
            {
                foreach (var _ in itemTeam.Players)
                {
                    counter++;
                }
            }

            return counter == 4;
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

            // Add new Player
            if (Groups.Count == 0)
            {
                Groups.Add(new Group { Id = Guid.NewGuid().ToString() });
                Groups[0].Teams.Add(new Team { Id = Guid.NewGuid().ToString() });
                Groups[0].Teams[0].Players.Add(inputPlayer);
                return Groups[0];
            }

            var lastGroup = Groups[Groups.Count - 1];

            if (lastGroup.Teams.Count == 2 && lastGroup.Teams[1].Players.Count == 2)
            {
                Groups.Add(new Group { Id = Guid.NewGuid().ToString() });
                Groups[Groups.Count - 1].Teams.Add(new Team { Id = Guid.NewGuid().ToString() });
                Groups[Groups.Count - 1].Teams[0].Players.Add(inputPlayer);
                return Groups[Groups.Count - 1];
            }

            if (lastGroup.Teams.Count == 1)
            {
                if (lastGroup.Teams[0].Players.Count < 2)
                {
                    lastGroup.Teams[0].Players.Add(inputPlayer);
                    return lastGroup;
                }

                lastGroup.Teams.Add(new Team { Id = Guid.NewGuid().ToString() });
                lastGroup.Teams[1].Players.Add(inputPlayer);
                return lastGroup;
            }

            if (lastGroup.Teams.Count == 2)
            {
                if (lastGroup.Teams[1].Players.Count < 2)
                {
                    lastGroup.Teams[1].Players.Add(inputPlayer);
                    return lastGroup;
                }
            }

            return null;
        }
    }
}
