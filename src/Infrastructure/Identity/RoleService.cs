using Microsoft.AspNetCore.Identity;
using Clean_Architecture.Application.Common.Interfaces;
using Clean_Architecture.Application.Common.Models;
using Clean_Architecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clean_Architecture.Infrastructure.Identity;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleService(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<RoleGroup?> GetRoleByIdAsync(int id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null) return null;

        return new RoleGroup
        {
            Id = int.Parse(role.Id),
            Name = role.Name,
            Description = role.NormalizedName
        };
    }

    public async Task<List<RoleGroup>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var roleGroups = new List<RoleGroup>();

        foreach (var role in roles)
        {
            roleGroups.Add(new RoleGroup
            {
                Id = int.Parse(role.Id),
                Name = role.Name,
                Description = role.NormalizedName 
            });
        }

        return roleGroups;
    }

    public async Task<Result> CreateRoleAsync(RoleGroup role)
    {
        if (role.RoleUuid == null)
        {
            role.RoleUuid = Guid.NewGuid().ToString();
        }
        
        var applicationRole = new ApplicationRole
        {
            Id = role.RoleUuid, Name = role.Name, Description = role.Description 
        };

        var result = await _roleManager.CreateAsync(applicationRole);

        return result.ToApplicationResult();
    }

    public async Task<Result> UpdateRoleAsync(RoleGroup role)
    {
        if (role.RoleUuid == null)
        {
            return Result.Failure(new List<string> { "Role not found." });
        }
        
        var existingRole = await _roleManager.FindByIdAsync(role.RoleUuid.ToString());
        if (existingRole == null)
        {
            return Result.Failure(new List<string> { "Role not found." });
        }

        existingRole.Name = role.Name;
        existingRole.NormalizedName = role.Description; 

        var result = await _roleManager.UpdateAsync(existingRole);

        return result.ToApplicationResult();
    }

    public async Task<Result> DeleteRoleAsync(string uuid)
    {
        var role = await _roleManager.FindByIdAsync(uuid);
        if (role == null)
        {
            return Result.Failure(new List<string> { "Role not found." });
        }

        var result = await _roleManager.DeleteAsync(role);

        return result.ToApplicationResult();
    }
}
