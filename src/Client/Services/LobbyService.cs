using Client.Models;

namespace Client.Services
{
    public class LobbyService
    {
        private readonly List<Group> _groups = new List<Group>();


        public Guid AddUser(User user)
        {
            if (_groups.Count() == 0)
            {
                Group group = new Group { GroupId = Guid.NewGuid()};

                group.Teams.Add(new Team { TeamId = Guid.NewGuid() });
                group.Teams.Add(new Team { TeamId = Guid.NewGuid() });

                group.Teams[0].Users.Add(user);

                _groups.Add(group);

                return group.GroupId;
            }

            foreach (var group in _groups)
            {
                if (group.Teams[0].Users.Count() < 2)
                {
                    if (group.Teams[0].Users.FirstOrDefault(_ => _.UserId ==user.UserId) == null
                        && group.Teams[1].Users.FirstOrDefault(_ => _.UserId == user.UserId) == null)
                    {
                        group.Teams[0].Users.Add(user);
                        return group.GroupId;
                    }
                }
                else if (group.Teams[1].Users.Count() < 2)
                {
                    if (group.Teams[0].Users.FirstOrDefault(_ => _.UserId == user.UserId) == null
                        && group.Teams[1].Users.FirstOrDefault(_ => _.UserId == user.UserId) == null)
                    {
                        group.Teams[1].Users.Add(user);
                        return group.GroupId;
                    }
                }
            }

            Group ngroup = new Group { GroupId = Guid.NewGuid() };

            ngroup.Teams.Add(new Team { TeamId = Guid.NewGuid() });
            ngroup.Teams.Add(new Team { TeamId = Guid.NewGuid() });

            ngroup.Teams[0].Users.Add(user);

            _groups.Add(ngroup);

            return ngroup.GroupId;
        }

        public Group GetFullGroup(string groupId)
        {
            var group = _groups.FirstOrDefault(_ => _.GroupId.ToString() == groupId);

            if (group != null && group.Teams[0].Users.Count == 2 && group.Teams[1].Users.Count == 2)
            {
                return group;
            }

            return null;
        }

        public Group GetGroup(string groupId)
        {
            var group = _groups.FirstOrDefault(_ => _.GroupId.ToString() == groupId);

            return group;
        }
    }
}
