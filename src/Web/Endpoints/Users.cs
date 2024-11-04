using Clean_Architecture.Application.Common.Models;
using Clean_Architecture.Infrastructure.Identity;

namespace Clean_Architecture.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MyMapIdentityApi<ApplicationUser>();
    }
}
