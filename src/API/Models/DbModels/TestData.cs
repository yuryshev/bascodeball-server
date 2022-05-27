namespace API.Models.DbModels;

public class TestData
{
    public Guid TestDataId { get; set; }
    
    public string InputData { get; set; }
    
    public string ExpectedResult { get; set; }
}