namespace Domain.Entities
{
    public class Hour
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public float Hours { get; set; }
        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; } = null!;
        public Guid? ContractorId { get; set; }
        public Contractor? Contractor { get; set; }
        public string? Comment { get; set; }
    }
}
