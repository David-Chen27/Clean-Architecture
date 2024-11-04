using NUlid;

namespace Clean_Architecture.Domain.Entities;

public class AccountRoleGroup : BaseAuditableEntity
{
    public int AccountId { get; set; }
    
    public int RoleGroupId { get; set; }
    
    public Account? Account { get; set; }
    
    public RoleGroup? RoleGroup { get; set; }
}
