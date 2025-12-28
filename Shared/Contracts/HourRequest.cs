using WebApplication1.Enties;

namespace Shared.Contracts
{
    public class HourRequest
    {
        public DateTime Date { get; set; }
        public float Hours { get; set; }
        public int WorkerId { get; set; }

    }
}
