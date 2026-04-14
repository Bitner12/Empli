using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{

    public class Worker
    {

        public Guid Id { get; set; } 
      
        public string? Pesel { get; set; }

        public Guid? CompanyId { get; set; }

        public Employee? Employee { get; set; }

        public Guid? EmployeeId { get; set; }

        public Company? Company { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public decimal? CostPerHour { get; set; }

        public virtual ICollection<Hour> Hours { get; set; }



    }

}
