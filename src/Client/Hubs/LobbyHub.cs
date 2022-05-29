using Client.Models;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Lobby.Hubs
{
    [Authorize]
    public class LobbyHub : Hub
    {
        private readonly LobbyService _lobbyService;

        public LobbyHub(LobbyService lobbyService)
        {
            _lobbyService = lobbyService;
        }

        public async Task Join(string userJson)
        {
            Console.WriteLine(userJson);
            var user = JsonSerializer.Deserialize<User>(userJson);

            string groupId = _lobbyService.AddUser(user).ToString();
            Console.WriteLine(groupId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);


            var group = _lobbyService.GetFullGroup(groupId);
            if (group == null)
            {
                group = _lobbyService.GetGroup(groupId);
                await Clients.Group(groupId).SendAsync("Receive", group);
            }
            else
            {
               
                // Send API
                // Get First Task
                await Clients.Group(groupId).SendAsync("Receive", group);
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Receive", $"{Context.User.Identity.Name} вошел в чат", "asd");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Receive", $"{Context.User.Identity.Name} покинул в чат", "asd");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
