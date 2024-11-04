namespace Clean_Architecture.Domain.Constants;

public abstract class Resources
{
    public const string News = nameof(News);
    
    private static readonly Dictionary<string, string> ResourceDescriptions = new()
    {
        { News, "最新消息" },
    };

    public static string GetDescription(string actionName)
    {
        return ResourceDescriptions.TryGetValue(actionName, out var description) ? description : actionName;
    }
}
