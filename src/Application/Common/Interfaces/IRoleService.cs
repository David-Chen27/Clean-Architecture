using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Application.Common.Models;

namespace Clean_Architecture.Application.Common.Interfaces;

public interface IRoleService
{
    Task<RoleGroup?> GetRoleByIdAsync(int id);
    Task<List<RoleGroup>> GetAllRolesAsync();
    Task<Result> CreateRoleAsync(RoleGroup role);
    Task<Result> UpdateRoleAsync(RoleGroup role);
    Task<Result> DeleteRoleAsync(string uuid);
}
