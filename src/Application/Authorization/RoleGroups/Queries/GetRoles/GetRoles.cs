using Clean_Architecture.Application.Common.Behaviours;
using Clean_Architecture.Application.Common.Interfaces;

namespace Clean_Architecture.Application.Authorization.RoleGroups.Queries.GetRoles;

[LoggingBehavior]
public class GetRolesQuery : IRequest<List<RoleVm>>;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleVm>>
{
    private readonly IApplicationDbContext _context;

    public GetRolesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoleVm>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _context
            .RoleGroups
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .ThenInclude(p => p!.Resource)
            .ToListAsync(cancellationToken);

        var roleVms = roles.Select(r => new RoleVm
        {
            Role = new RoleDto { Name = r.Name, Description = r.Description, Uuid = r.Uuid.ToString() },
            Resources = r.RolePermissions
                .GroupBy(rp => rp.Permission!.Resource)
                .Select(g => new ResourceDto
                {
                    Name = g.Key!.Name,
                    Description = g.Key.Description,
                }).ToList()
        }).ToList();

        return roleVms;
    }
}
