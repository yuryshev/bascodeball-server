using System.Text.Json;
using System.Text.Json.Serialization;
using Common.DbModels;
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

        public async Task<Exercise> GetRandomTask()
        {
            var taskHttpResponseMessage = await this._factory.CreateClient().GetAsync("https://localhost:5001/getExercise");
            var task = await taskHttpResponseMessage.Content.ReadFromJsonAsync<Exercise>();

            return task;
        }
    }
}
