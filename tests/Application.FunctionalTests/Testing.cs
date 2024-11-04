using Clean_Architecture.Domain.Constants;
using Clean_Architecture.Infrastructure.Data;
using Clean_Architecture.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Clean_Architecture.Domain.Entities;
using Clean_Architecture.Domain.Extensions;

namespace Clean_Architecture.Application.FunctionalTests;

[SetUpFixture]
public partial class Testing
{
    private static ITestDatabase _database = null!;
    private static CustomWebApplicationFactory _factory = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static string? _userId;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        _database = await TestDatabaseFactory.CreateAsync();

        _factory = new CustomWebApplicationFactory(_database.GetConnection());

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task SendAsync(IBaseRequest request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public static string? GetUserId()
    {
        return _userId;
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
    }

    public static async Task<string> RunAsAdministratorAsync()
    {
        return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { Roles.Administrator });
    }

    public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new ApplicationUser { UserName = userName, Email = userName };

        var result = await userManager.CreateAsync(user, password);

        if (roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new ApplicationRole(role));
            }

            await userManager.AddToRolesAsync(user, roles);
        }

        // Default Resources
        if (!context.Resources.Any())
        {
            var addResources = new List<Resource>();
            var resources = typeof(Domain.Constants.Resources).GetAllConstants();

            foreach (var resource in resources)
            {
                addResources.Add(new Resource { Name = resource, Description = resource });
            }

            context.Resources.AddRange(addResources);
            await context.SaveChangesAsync();
        }

        // Default Permissions
        if (!context.Permissions.Any())
        {
            var addPermissions = new List<Permission>();
            var resources = context.Resources.ToList();
            var permissions = typeof(Actions).GetAllConstants();

            foreach (var resource in resources)
            {
                foreach (var permission in permissions)
                {
                    addPermissions.Add(new Permission
                    {
                        ResourceId = resource.Id, Name = permission, Description = permission
                    });
                }
            }

            context.Permissions.AddRange(addPermissions);
            await context.SaveChangesAsync();

            // Default RolePermissions
            if (!context.RolePermissions.Any())
            {
                var addRolePermissions = new List<RolePermission>();

                var adminstratorRoleId = context.Roles.Where(p => p.Name == Roles.Administrator).Select(p => p.Id)
                    .FirstOrDefault();

                if (adminstratorRoleId != null)
                {
                    foreach (var permission in context.Permissions)
                    {
                        addRolePermissions.Add(new RolePermission
                        {
                            RoleId = adminstratorRoleId, PermissionId = permission.Id
                        });
                    }

                    context.RolePermissions.AddRange(addRolePermissions);
                    await context.SaveChangesAsync();
                }
            }
        }

        if (result.Succeeded)
        {
            _userId = user.Id;

            return _userId;
        }

        var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
    }

    public static async Task ResetState()
    {
        try
        {
            await _database.ResetAsync();
        }
        catch (Exception) 
        {
        }

        _userId = null;
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        await _database.DisposeAsync();
        await _factory.DisposeAsync();
    }
}
