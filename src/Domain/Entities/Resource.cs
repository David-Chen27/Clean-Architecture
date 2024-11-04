namespace Clean_Architecture.Domain.Entities;

public class Resource : BaseAuditableEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
