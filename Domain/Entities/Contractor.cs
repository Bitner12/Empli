namespace Domain.Entities;

public class Contractor
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
    public decimal RatePerHour { get; set; }
    public Company Company { get; set; } = null!;
    public List<Hour>? Hours { get; set; }
}
