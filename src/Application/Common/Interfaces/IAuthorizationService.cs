using Clean_Architecture.Application.Common.Models;

namespace Clean_Architecture.Application.Common.Interfaces;

public interface IApplicationAuthorizationService
{
    Task<Result> IsInRoleAsync(string userId, string role);

    Task<Result> AuthorizeAsync(string userId, string policyName);
}
