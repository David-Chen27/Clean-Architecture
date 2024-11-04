using Microsoft.AspNetCore.Identity;

namespace Clean_Architecture.Infrastructure.Identity;

public sealed class ApplicationRole : IdentityRole
{
    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }
    
    public string? Description { get; set; }
}
