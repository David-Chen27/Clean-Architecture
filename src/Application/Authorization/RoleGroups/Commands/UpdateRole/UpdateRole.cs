using Microsoft.EntityFrameworkCore.Storage;
using Clean_Architecture.Application.Common.Interfaces;
using Clean_Architecture.Application.Common.Models;
using NUlid;

namespace Clean_Architecture.Application.Authorization.RoleGroups.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest<Result>
    {
        public string? Uuid { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        private readonly IRoleService _roleService;

        public UpdateRoleCommandHandler(IApplicationDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var entity = await _context.RoleGroups.Where(s => s.Uuid == Ulid.Parse(request.Uuid))
                        .FirstOrDefaultAsync(cancellationToken);

                    if (entity == null)
                    {
                        return Result.Failure(new List<string> { "Role not found." });
                    }

                    entity.Name = request.Name;
                    entity.Description = request.Description;

                    await _context.SaveChangesAsync(cancellationToken);

                    var identityResult = await _roleService.UpdateRoleAsync(entity);

                    if (!identityResult.Succeeded)
                    {
                        await transaction.RollbackAsync(cancellationToken);
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
}
