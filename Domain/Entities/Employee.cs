using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string Pesel { get; set; }

    public string? FirstName { get; set; } 
    
    public string? LastName { get; set; } 
    
    public string? UserId {get; set;}

    public User? User { get; set; }

    public Guid? WorkerId { get; set; }

    public Worker? Worker { get; set; }

    public List<Hour>? Hours { get; set; }
    
}