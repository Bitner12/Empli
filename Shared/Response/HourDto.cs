namespace Shared.Response
{
    public class HourDto
    {
        public DateTime Date { get; set; }
        public float Hours { get; set; }
        public Guid Id { get; set; }
        public Guid? ContractorId { get; set; }
        public string? ContractorName { get; set; }
        public string? Comment { get; set; }
    }
}
