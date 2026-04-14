

using System.ComponentModel.DataAnnotations;

namespace Shared.Contracts
{

    public class WorkerRequest
    {

        
        [StringLength(11, MinimumLength = 11)]
        public string? Pesel { get; set; }
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public decimal CostPerHour { get; set; }

    }

}
