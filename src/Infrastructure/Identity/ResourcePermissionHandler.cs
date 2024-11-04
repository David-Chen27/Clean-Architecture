using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Clean_Architecture.Infrastructure.Data;

namespace Clean_Architecture.Infrastructure.Identity;

public class ResourcePermissionRequirement : IAuthorizationRequirement
{
    public string Resource { get; }
    public string Permission { get; }

    public ResourcePermissionRequirement(string resource, string permission)
    {
        Resource = resource;
        Permission = permission;
    }
}

public class ResourcePermissionHandler : AuthorizationHandler<ResourcePermissionRequirement>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ResourcePermissionHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ResourcePermissionRequirement requirement)
    {
        var user = await _userManager.GetUserAsync(context.User);

        if (user == null) return;

        var roles = await _userManager.GetRolesAsync(user);
        
        var roleIds = await _context.Roles
            .Where(r => roles.Contains(r.Name!))
            .Select(r => r.Id)
            .ToListAsync();

        var isPermissionExist = await _context.RolePermissions
            .Include(rp => rp.Permission)
            .ThenInclude(p => p!.Resource)
            .Where(rp => roleIds.Contains(rp.RoleId!))
            .Where(rp => rp.Permission!.Name == requirement.Permission)
            .Where(rp => rp.Permission!.Resource!.Name == requirement.Resource)
            .AnyAsync();

        if (isPermissionExist)
        {
            context.Succeed(requirement);
        }
    }
}
