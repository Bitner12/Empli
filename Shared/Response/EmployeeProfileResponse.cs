namespace Shared.Response;

public class EmployeeProfileResponse
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Pesel { get; set; }
    public decimal? CostPerHour { get; set; }
    public Guid? WorkerId { get; set; }
    public bool IsLinkedToCompany { get; set; }
}
