using Application.Abstratctions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    

    public async Task<User> UpdateUser(User user, UserType userType,Guid userTypeId , Company? company, Employee? employee)
    {
        user.UserType = userType;
        if (userType == UserType.Company)
        {
            user.CompanyId = userTypeId;
            user.Company = company;
            user.EmployeeId = null;
            user.Employee = null;
        }
        else if (userType == UserType.Employee)
        {
            user.EmployeeId = userTypeId;
            user.Employee = employee;
            user.CompanyId = null;
            user.Company = null;
        }

        await _userManager.UpdateAsync(user);
        return user;
    }


    public async Task<string> DeleteUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return "Invalid user id.";
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return "User not found.";
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return "User deleted successfully.";
        }
        else
        {
            var errorMessages = result.Errors?.Select(e => e.Description) ?? new[] { "Unknown error" };
            return "Error deleting user: " + string.Join(", ", errorMessages);
        }
    }





}