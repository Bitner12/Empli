namespace Shared.Response;

public class UserProfileDto
{
    public int UserType { get; set; }
    public string? Email { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? WorkerId { get; set; }
}
