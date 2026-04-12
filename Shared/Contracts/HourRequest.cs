

namespace Shared.Contracts
{
    public class HourRequest
    {
        public DateTime Date { get; set; }
        public float Hours { get; set; }
        public Guid WorkerId { get; set; }

    }
}
