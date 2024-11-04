using Clean_Architecture.Application.Authorization.Resources.Queries.GetResources;

namespace Clean_Architecture.Web.Endpoints;

public class Resources : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetResources, "/Permissions");
    }

    public async Task<List<ResourceVm>> GetResources(ISender sender)
    {
        return await sender.Send(new GetResourcesQuery());
    }
}
