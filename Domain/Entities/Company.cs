namespace Domain.Entities;

public class Company
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
     
    public string? Nip  { get; set; }
    
    public string? UserId { get; set; }
    
    public User? User { get; set; }
    
    public List<Worker>? Workers { get; set; }
    public List<Contractor>? Contractors { get; set; }
}