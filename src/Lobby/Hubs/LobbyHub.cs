using Lobby.Models;
using Lobby.Services;
using Microsoft.AspNetCore.SignalR;

namespace Lobby.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly GroupService _groups;
        private readonly CodeTaskService _tasks;

        public LobbyHub(GroupService groups, CodeTaskService tasks)
        {
            _groups = groups;
            _tasks = tasks;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"{DateTime.Now} - Client {Context.ConnectionId} connected.");
            await Clients.Caller.SendAsync("ReciveConnectionId", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"{DateTime.Now} - Client {Context.ConnectionId} disconected.");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Join(Player player)
        {
            Group group = _groups.AddPlayer(player);

            if (_groups.IsFull(group.Id) && group.Task == null)
            {
                var task = _tasks.GetRandomTask();
                group.Task = task;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id);
            await Clients.Group(group.Id).SendAsync("ReciveGroup", group);
        }

        public async Task SendCodeblock(Codeblock codeblock)
        {
            Console.WriteLine($"{codeblock.Id} {codeblock.Code}");
            await Clients.GroupExcept(codeblock.GroupId, codeblock.ConnectionId).SendAsync("ReceiveCodeblock", codeblock);
        }

    }
}
