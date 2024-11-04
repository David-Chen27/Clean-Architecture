using AutoMapper;
using Clean_Architecture.Application.Common.Models;
using Clean_Architecture.Application.Authorization.RoleGroups.Commands.CreateRole;
using Clean_Architecture.Application.Authorization.RoleGroups.Commands.DeleteRole;
using Clean_Architecture.Application.Authorization.RoleGroups.Commands.UpdateRole;
using Clean_Architecture.Application.Authorization.RoleGroups.Queries.FindRoleByUuid;
using Clean_Architecture.Application.Authorization.RoleGroups.Queries.GetRoles;
using Clean_Architecture.Infrastructure.Identity;

namespace Clean_Architecture.Web.Endpoints;

public class Roles : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetRoleByUuid, "{uuid}")
            .MapPost(CreateRole)
            .MapGet(GetRoles)
            .MapPut(UpdateRole, "{uuid}")
            .MapDelete(DeleteRole, "{uuid}");
    }
    
    public async Task<IResult> GetRoleByUuid(ISender sender, string uuid)
    {
        var query = new FindRoleByUuidQuery() {Uuid = uuid};
        var role = await sender.Send(query);
        return Results.Ok(role);
    }
    
    public async Task<IResult> UpdateRole(ISender sender, string uuid, UpdateRoleCommand command)
    {
        if (uuid != command.Uuid) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
    
    public async Task<Result> CreateRole(ISender sender, CreateRoleCommand command)
    {
        return await sender.Send(command);
    }
    
    public async Task<IResult> GetRoles(ISender sender)
    {
        var query = new GetRolesQuery();
        var role = await sender.Send(query);
        return Results.Ok(role);
        
        //return await applicationDbContext.Roles
        //    .Where(s => s.Name != Domain.Constants.Roles.Administrator)
        //    .ProjectTo<RoleDto>(applicationDbContext.GetService<IMapper>().ConfigurationProvider)
        //    .ToListAsync();
    }
    
    public async Task<IResult> DeleteRole(ISender sender, string uuid)
    {
        await sender.Send(new DeleteRoleCommand { Uuid = uuid });
        return Results.NoContent();
    }
}

public class RoleDto
{
    public string? Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? NormalizedName { get; set; }
    
    public string? Description { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationRole, RoleDto>();
        }
    }
}
