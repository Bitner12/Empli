using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{

    public class Worker
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal CostPerHour { get; set; }

        public virtual ICollection<Hour> Hours { get; set; }



    }

}
