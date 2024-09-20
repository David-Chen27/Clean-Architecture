using System.Security.Claims;

using Clean_Architecture.Application.Common.Interfaces;

namespace Clean_Architecture.Web.Services;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    
    public string? IpAddress => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
    
    public string? UserAgent => _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"];
    
    public string? TraceId => _httpContextAccessor.HttpContext?.TraceIdentifier;
}
