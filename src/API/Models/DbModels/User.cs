using System;

namespace API.Models.DbModels;
public class User
{
    public Guid UserId { get; set; }
    
    public string LoginName { get; set; }
    
    public string Email { get; set; }

    public string Role { get; set; }
    
    public int Rating { get; set; }
    
}