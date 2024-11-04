namespace Clean_Architecture.Application.Authorization.RoleGroups.Queries.FindRoleByUuid;

public class RoleVm
{
    public RoleDto? Role { get; init; }

    public List<ResourceDto>? Resources { get; init; }
}
