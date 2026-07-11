namespace Shared.Contracts
{
    public class HourRequest
    {
        public DateTime Date { get; set; }
        public float Hours { get; set; }
        public Guid WorkerId { get; set; }
        public Guid? ContractorId { get; set; }
        public string? Comment { get; set; }
    }
}
