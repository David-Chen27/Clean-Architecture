using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Clean_Architecture.Domain.Extensions;

namespace Clean_Architecture.Infrastructure.Identity;

using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Clean_Architecture.Domain.Constants;

public class CustomizePolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

    public CustomizePolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync();

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var permissionRequirement = ParsePolicyName(policyName);

        var policyIsExists = policyExists(permissionRequirement.Resource);

        if (!policyIsExists)
        {
            return await _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        var policyBuilder = new AuthorizationPolicyBuilder();
        
        policyBuilder = policyBuilder.AddRequirements(
            new ResourcePermissionRequirement(permissionRequirement.Resource, permissionRequirement.Permission)
        );

        return policyBuilder.Build();
    }

    private bool policyExists(string policyName)
    {
        var policies = GetAllResourceConstants();

        return policies.Contains(policyName);
    }

    private ResourcePermissionRequirement ParsePolicyName(string policyName)
    {
        var parts = policyName.Split('.');
        if (parts.Length != 2)
        {
            return new ResourcePermissionRequirement("", "");
        }

        return new ResourcePermissionRequirement(parts[0], parts[1]);
    }

    private IEnumerable<string> GetAllResourceConstants()
    {
        return typeof(Resources).GetAllConstants();
    }
}
