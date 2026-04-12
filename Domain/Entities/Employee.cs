namespace Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } 
    
    public string LastName { get; set; } 
    
    public string UserId {get; set;}
    
    public User User { get; set; }
    
    public List<Hour> Hours { get; set; }
    
}