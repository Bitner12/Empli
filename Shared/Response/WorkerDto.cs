

namespace Shared.Response
{
    public class WorkerDto
    {
        public Guid Id { get; set; }
        public string Pesel { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public decimal? CostPerHour { get; set; }

        public virtual ICollection<HourDto>? Hours { get; set; }

        public float? TotalHours { get; set; }
        public decimal? TotalCost {  get; set; }
        
    }
}
