using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{

    public class Worker
    {

        public Guid Id { get; set; }
        
        public Guid CompanyId { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public decimal CostPerHour { get; set; }

        public virtual ICollection<Hour> Hours { get; set; }



    }

}
