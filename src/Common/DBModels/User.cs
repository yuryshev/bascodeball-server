namespace Common.DbModels;
public class User
{
    public Guid UserId { get; set; }
    
    public string ConnectionId { get; set; }
    
    public string NickName { get; set; }

    public string Picture { get; set; }
    
    public string Email { get; set; }

    public string Role { get; set; }
    
    public int Rating { get; set; }
}