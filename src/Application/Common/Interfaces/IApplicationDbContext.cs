using Microsoft.EntityFrameworkCore.Infrastructure;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DatabaseFacade Database { get; }
    
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<Permission> Permissions { get; }
    
    DbSet<RolePermission> RolePermissions { get; }
    
    DbSet<Resource> Resources { get; }
    
    DbSet<Account> Accounts { get; }
    
    DbSet<RoleGroup> RoleGroups { get; set; }
    
    DbSet<AccountRoleGroup> AccountRoleGroups { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
