namespace Common.DbModels;

public class Exercise
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public ICollection<TestData> Tests { get; set; }
}