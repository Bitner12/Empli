

namespace Shared.Contracts
{
    public class EmployeeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }

        public decimal? CostPerHour { get; set; }

        public string? NipCompany { get; set; }

    }
}
