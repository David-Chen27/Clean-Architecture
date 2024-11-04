namespace Clean_Architecture.Domain.Entities;

public class Permission : BaseAuditableEntity
{
    public int ResourceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public Resource? Resource { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
