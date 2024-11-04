namespace Clean_Architecture.Application.Authorization.RoleGroups.Queries.FindRoleByUuid;

public class ResourceDto
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public List<PermissionDto>? Permissions { get; set; }
}
