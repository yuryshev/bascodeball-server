using Common.DTOModels;
using Common.DbModels;
using Lobby.Models;
using Lobby.Services;
using Microsoft.AspNetCore.SignalR;

namespace Lobby.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly GroupService _groups;
        private readonly CodeTaskService _tasks;
        private readonly IHttpClientFactory _httpClientFactory;

        private const int VictoryPoints = 30;

        public LobbyHub(GroupService groups, CodeTaskService tasks, IHttpClientFactory httpClientFactory)
        {
            _groups = groups;
            _tasks = tasks;
            _httpClientFactory = httpClientFactory;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"{DateTime.Now} - Client {Context.ConnectionId} connected.");
            await Clients.Caller.SendAsync("ReceiveConnectionId", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"{DateTime.Now} - Client {Context.ConnectionId} disconnected.");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Join(User player)
        {
            Group group = _groups.AddPlayer(player);

            if (_groups.IsFull(group.Id) && group.Task == null)
            {
                var task = await _tasks.GetRandomTask();
                group.Task = task;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id);
            await Clients.Group(group.Id).SendAsync("ReceiveGroup", group);
        }

        public async Task Win(string groupId, string winTeamId)
        {
            Group group = _groups.Groups!.FirstOrDefault(g => g.Id == groupId)!;
            Team team = group.Teams!.FirstOrDefault(t => t.Id == winTeamId)!;


            var httpClient = _httpClientFactory.CreateClient();
            var updatePlayerRatingCollection = new List<UserDTO>();

            foreach (var teamItem in group.Teams)
            {
                foreach (var playerItem in teamItem.Players)
                {
                    if (teamItem.Id == winTeamId)
                    {
                        updatePlayerRatingCollection.Add(new UserDTO { Email = playerItem.Email, Rating = playerItem.Rating + VictoryPoints});
                    }
                    else
                    {
                        if (playerItem.Rating - VictoryPoints >= 0)
                        {
                            updatePlayerRatingCollection.Add(new UserDTO { Email = playerItem.Email, Rating = playerItem.Rating - VictoryPoints });
                        }
                    }
                }
            }

            await httpClient.PostAsJsonAsync("https://localhost:5001/updateUserRating", updatePlayerRatingCollection);
            await Clients.Group(group.Id).SendAsync("ReceiveWin", team.Id);
            _groups.Groups.Remove(group);
        }

        public async Task SendCodeblock(Codeblock codeblock)
        {
            Console.WriteLine($"{codeblock.Id} {codeblock.Code}");
            await Clients.GroupExcept(codeblock.GroupId, codeblock.ConnectionId).SendAsync("ReceiveCodeblock", codeblock);
        }

    }
}
