namespace Shared.Contracts
{
    public class ContractorRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal RatePerHour { get; set; }
    }
}