using Microsoft.EntityFrameworkCore.Storage;
using Clean_Architecture.Application.Common.Interfaces;
using Clean_Architecture.Application.Common.Models;
using Clean_Architecture.Domain.Entities;

namespace Clean_Architecture.Application.Authorization.RoleGroups.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<Result>
    {
        public required string Name { get; set; }
        
        public required string Description { get; set; }
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IRoleService _roleService;

        public CreateRoleCommandHandler(IApplicationDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var guid = Guid.NewGuid().ToString();
                    
                    var identityResult = await _roleService.CreateRoleAsync(new RoleGroup() {RoleUuid = guid, Name = request.Name, Description = request.Description });

                    if (!identityResult.Succeeded)
                    {
                        return identityResult;
                    }

                    var entity = new RoleGroup() {RoleUuid = guid, Name = request.Name, Description = request.Description};

                    _context.RoleGroups.Add(entity);

                    await _context.SaveChangesAsync(cancellationToken);

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
