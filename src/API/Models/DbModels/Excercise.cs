namespace API.Models.DbModels;

public class Exercise
{
    public Guid ExerciseId { get; set; }
    
    public string Description { get; set; }
    
    public string FunctionName { get; set; }
    
    public ICollection<TestData> TestingData { get; set; }
}