using Application.Abstratctions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    
    public async Task<User> GetUser(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
    

    public async Task<User> UpdateUser(User user, UserType userType)
    {
        user.UserType = userType;
        
        await _userManager.UpdateAsync(user);
        return user;
    }
    
    
    
    
    
}