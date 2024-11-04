using MediatR;
using Clean_Architecture.Domain.Constants;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Clean_Architecture.Application.Authorization.RoleGroups.Commands.CreateRole;
using Clean_Architecture.Domain.Extensions;

namespace Clean_Architecture.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ISender _sender;

    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ISender sender
    )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _sender = sender;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();

        var roleConstants = typeof(Roles).GetAllConstants();

        foreach (var role in roleConstants)
        {
            if (!roles.Contains(role))
            {
                var createRoleCommand = new CreateRoleCommand{Name = role, Description = Roles.GetDescription(role)};
                await _sender.Send(createRoleCommand);
            }
        }

        var administratorRole = new ApplicationRole(Roles.Administrator);

        // Default users
        var administrator =
            new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Default Resources
        if (!_context.Resources.Any())
        {
            var addResources = new List<Resource>();
            var resources = typeof(Domain.Constants.Resources).GetAllConstants();

            foreach (var resource in resources)
            {
                addResources.Add(new Resource { Name = resource, Description = Resources.GetDescription(resource) });
            }

            _context.Resources.AddRange(addResources);
            await _context.SaveChangesAsync();
        }

        // Default Permissions
        if (!_context.Permissions.Any())
        {
            var addPermissions = new List<Permission>();
            var resources = _context.Resources.ToList();
            var permissions = typeof(Actions).GetAllConstants();

            foreach (var resource in resources)
            {
                foreach (var permission in permissions)
                {
                    addPermissions.Add(new Permission
                    {
                        ResourceId = resource.Id, Name = permission, Description = Actions.GetDescription(permission)
                    });
                }
            }

            _context.Permissions.AddRange(addPermissions);
            await _context.SaveChangesAsync();
        }

        // Default RolePermissions
        if (!_context.RolePermissions.Any())
        {
            var addRolePermissions = new List<RolePermission>();

            var adminstratorRoleId = _context.Roles.Where(p => p.Name == Roles.Administrator).Select(p => p.Id)
                .FirstOrDefault();

            if (adminstratorRoleId != null)
            {
                foreach (var permission in _context.Permissions)
                {
                    addRolePermissions.Add(new RolePermission
                    {
                        RoleId = adminstratorRoleId, PermissionId = permission.Id
                    });
                }

                _context.RolePermissions.AddRange(addRolePermissions);
                await _context.SaveChangesAsync();
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }
    }
}
