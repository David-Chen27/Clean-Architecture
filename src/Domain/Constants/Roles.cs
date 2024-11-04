namespace Clean_Architecture.Domain.Constants;

public abstract class Roles
{
    public const string Administrator = nameof(Administrator);

    public const string Manager = nameof(Manager);

    public const string Pharmacy = nameof(Pharmacy);

    private static readonly Dictionary<string, string> RoleDescriptions = new()
    {
        { Administrator, "最高權限管理員" },
        { Manager, "管理員" },
        { Pharmacy, "藥局" }
    };

    public static string GetDescription(string roleName)
    {
        return RoleDescriptions.TryGetValue(roleName, out var description) ? description : roleName;
    }
}
