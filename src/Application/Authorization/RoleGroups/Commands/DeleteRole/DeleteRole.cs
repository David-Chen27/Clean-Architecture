using Microsoft.EntityFrameworkCore.Storage;
using Clean_Architecture.Application.Common.Interfaces;
using Clean_Architecture.Application.Common.Models;
using NUlid;

namespace Clean_Architecture.Application.Authorization.RoleGroups.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest<Result>
{
    public string? Uuid { get; set; }
}

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IRoleService _roleService;

    public DeleteRoleCommandHandler(IApplicationDbContext context, IRoleService roleService)
    {
        _context = context;
        _roleService = roleService;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                var entity = await _context.RoleGroups.Where(s => s.Uuid == Ulid.Parse(request.Uuid)).FirstOrDefaultAsync(cancellationToken);

                if (entity == null)
                {
                    return Result.Failure(new List<string> { "Role not found." });
                }

                _context.RoleGroups.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);
                
                var identityResult = await _roleService.DeleteRoleAsync(entity.Uuid.ToString());

                if (!identityResult.Succeeded)
                {
                    return identityResult;
                }

                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
