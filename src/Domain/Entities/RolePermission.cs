namespace Clean_Architecture.Domain.Entities;

public class RolePermission : BaseAuditableEntity
{
    public required string RoleId { get; set; }
    public int PermissionId { get; set; }

    public Permission? Permission { get; set; }

    public RoleGroup? Role { get; set; }
}
