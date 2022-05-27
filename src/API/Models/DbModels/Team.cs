namespace API.Models.DbModels;

public class Team
{
    public Guid TeamId { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public ICollection<User> Players { get; set; }
    
    public ICollection<Exercise> SolvedExercises { get; set; }
}