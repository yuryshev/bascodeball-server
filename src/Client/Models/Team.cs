namespace Client.Models
{
    public class Team
    {
        public Guid TeamId { get; set; }

        public List<User> Users { get; set; } = new List<User>();
    }
}
