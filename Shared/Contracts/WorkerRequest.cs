using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Enties;

namespace Shared.Contracts
{

    public class WorkerRequest
    {
        public string Name { get; set; }

        public decimal CostPerHour { get; set; }

    }

}
