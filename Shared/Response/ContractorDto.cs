namespace Shared.Response
{
    public class ContractorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        public decimal RatePerHour { get; set; }
    }
}