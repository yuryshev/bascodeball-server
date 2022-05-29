namespace Client.Models
{
    public class Group
    {
        public Guid GroupId { get; set; }
        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
