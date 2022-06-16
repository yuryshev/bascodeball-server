namespace Lobby.Models
{
    public class CodeTask
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public List<CodeTaskTest> Tests { get; set; } = new List<CodeTaskTest>();
    }
}
