using System.Text.Json;
using System.Text.Json.Serialization;
using Lobby.Models;

namespace Lobby.Services
{
    public class CodeTaskService
    {
        private readonly IHttpClientFactory _factory;
        public CodeTaskService(IHttpClientFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<CodeTask> GetRandomTask()
        {
            var taskHttpResponseMessage = await this._factory.CreateClient().GetAsync("https://localhost:5001/get_exercise");
            var task = await taskHttpResponseMessage.Content.ReadFromJsonAsync<CodeTask>();

            return task;
        }

    }
}
