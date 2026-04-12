using Application.Abstratctions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class RegistrationService : IRegistrationService
    {

        private readonly UserManager<User> _userManager;
        
       

        public RegistrationService(UserManager<User> userManager)
        {
            _userManager = userManager;
     
        }


        public async Task<User> Registration(string email, string password)
        {
            var user = new User
            {
                UserName = email,
                Email = email,             
            };
            await _userManager.CreateAsync(user,password);

            return user;
        }
    }
}
