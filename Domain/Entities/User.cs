
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        
        public RefreshToken? RefreshToken { get; set; }
        
        
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }
        
        public Guid? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public UserType UserType { get; set; } = UserType.Empty;



    }
}