using NUlid;

namespace Clean_Architecture.Domain.Entities;

public class RoleGroup : BaseAuditableEntity
{
    public string? RoleUuid { get; set; }
    
    public Ulid Uuid { get; set; } = Ulid.NewUlid();
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public ICollection<AccountRoleGroup> AccountRoleGroups { get; set; } = new List<AccountRoleGroup>();
    
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
