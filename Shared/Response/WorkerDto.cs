

namespace Shared.Response
{
    public class WorkerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal CostPerHour { get; set; }

        public virtual ICollection<HourDto> Hours { get; set; }

        public decimal TotalHours { get; set; }
        public decimal TotalCost {  get; set; }
        
    }
}
