using Lobby.Models;

namespace Lobby.Services
{
    public class GroupService
    {
        public List<Group> Groups = new List<Group> { };

        public bool IsFull(string groupId)
        {
            var group = Groups.FirstOrDefault(_ => _.Id == groupId);

            int counter = 0;

            foreach(var itemTeam in group.Teams)
            {
                foreach (var itemPlayer in itemTeam.Players)
                {
                    counter++;
                }
            }

            return counter == 4;
        }

        public Group AddPlayer(Player? player)
        {
            // Reconect Player
            foreach (var _group in Groups)
            {
                foreach (var _team in _group.Teams)
                {
                    foreach (var _player in _team.Players)
                    {
                        if (_player.Id == player.Id && _player.ConnectionId != player.ConnectionId)
                        {
                            _player.ConnectionId = player.ConnectionId;
                            return _group;
                        }
                    }
                }
            }



            // Add new Player
            if (Groups.Count == 0)
            {
                Groups.Add(new Group { Id = Guid.NewGuid().ToString() });
                Groups[0].Teams.Add(new Team { Id = Guid.NewGuid().ToString() });
                Groups[0].Teams[0].Players.Add(player);
                return Groups[0];
            }

            var lastGroup = Groups[Groups.Count - 1];

            if (lastGroup.Teams.Count == 2 && lastGroup.Teams[1].Players.Count == 2)
            {
                Groups.Add(new Group { Id = Guid.NewGuid().ToString() });
                Groups[Groups.Count - 1].Teams.Add(new Team { Id = Guid.NewGuid().ToString() });
                Groups[Groups.Count - 1].Teams[0].Players.Add(player);
                return Groups[Groups.Count - 1];
            }

            if (lastGroup.Teams.Count == 1)
            {
                if (lastGroup.Teams[0].Players.Count < 2)
                {
                    lastGroup.Teams[0].Players.Add(player);
                    return lastGroup;
                }
                else
                {
                    lastGroup.Teams.Add(new Team { Id = Guid.NewGuid().ToString() });
                    lastGroup.Teams[1].Players.Add(player);
                    return lastGroup;
                }
            }

            if (lastGroup.Teams.Count == 2)
            {
                if (lastGroup.Teams[1].Players.Count < 2)
                {
                    lastGroup.Teams[1].Players.Add(player);
                    return lastGroup;
                }
            }

            return null;

        }
    }
}
