using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Clean_Architecture.Application.Common.Interfaces;
using Clean_Architecture.Application.Common.Models;
using Clean_Architecture.Application.Common.Requests;
using Clean_Architecture.Domain.Constants;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Infrastructure.Identity;

public class AccountService
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    
    public AccountService(IApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _context = applicationDbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result> RegisterAsync(AccountRegisterRequest request, string role = Roles.Manager)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
        {
            return Result.Failure(["Not found User."]);
        }

        var applicationRole = await _roleManager.FindByNameAsync(role);

        if (applicationRole == null)
        {
            return Result.Failure(["Role not found."]);
        }
        
        var accountEntity = new Account
        {
            Email = request.Email,
            ApplicationUserId = user.Id,
            RoleGroupId = applicationRole.Id,
            Status = false,
            UserName = request.UserName,
            Title = request.Title,
            TelEx = request.TelEx,
            Sex = request.Sex,
            IdNumber = request.IdNumber,
            Birthday = request.Birthday,
            Contact = request.Contact,
            Post = request.Post,
        };
        _context.Accounts.Add(accountEntity);

        await _userManager.AddToRoleAsync(user, role);
        
        await _context.SaveChangesAsync(CancellationToken.None);

        return Result.Success();
    }
}
