namespace Clean_Architecture.Application.Authorization.RoleGroups.Queries.GetRoles;

public class PermissionDto
{
    public int PermissionId { get; set; }
    public string? Name { get; set; }
    
    public string? Description { get; set; }
}
