using Clean_Architecture.Application.Common.Interfaces;
using Clean_Architecture.Domain.Entities;
using NUlid;

namespace Clean_Architecture.Application.Authorization.RoleGroups.Queries.FindRoleByUuid;

public class FindRoleByUuidQuery : IRequest<RoleVm>
{
    public string? Uuid { get; set; }
}

public class FindRoleByUuidQueryHandler : IRequestHandler<FindRoleByUuidQuery, RoleVm>
{
    private readonly IApplicationDbContext _context;

    public FindRoleByUuidQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoleVm> Handle(FindRoleByUuidQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Uuid))
        {
            throw new NotFoundException(nameof(RoleGroup), "Not found.");
        }

        var role = await _context
            .RoleGroups
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ThenInclude(p => p!.Resource)
            .FirstOrDefaultAsync(x => x.Uuid == Ulid.Parse(request.Uuid), cancellationToken);

        if (role is null)
        {
            throw new NotFoundException(nameof(RoleGroup), request.Uuid);
        }

        var roleVm = new RoleVm
        {
            Role = new RoleDto { Name = role.Name, Description = role.Description, Uuid = role.Uuid.ToString() },
            Resources = role.RolePermissions
                .GroupBy(rp => rp.Permission!.Resource)
                .Select(g => new ResourceDto
                {
                    Name = g.Key!.Name,
                    Description = g.Key.Description,
                    Permissions = g.Select(p => new PermissionDto
                    {
                        PermissionId = p.PermissionId,
                        Name = p.Permission!.Name,
                        Description = p.Permission.Description
                    }).ToList()
                }).ToList()
        };

        return roleVm;
    }
}
