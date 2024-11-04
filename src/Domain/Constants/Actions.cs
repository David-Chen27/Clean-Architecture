namespace Clean_Architecture.Domain.Constants;

public abstract class Actions
{
    /// <summary>
    /// 存取資料
    /// </summary>
    public const string Access = nameof(Access);
    
    /// <summary>
    /// 插入資料
    /// </summary>
    public const string Insert = nameof(Insert);
    
    /// <summary>
    /// 修改資料
    /// </summary>
    public const string Modify = nameof(Modify);
    
    /// <summary>
    /// 刪除資料
    /// </summary>
    public const string Delete = nameof(Delete);
    
    /// <summary>
    /// 執行
    /// </summary>
    public const string Execute = nameof(Execute);
    
    /// <summary>
    /// 提交
    /// </summary>
    public const string Commit = nameof(Commit);
    
    /// <summary>
    /// 回復
    /// </summary>
    public const string Rollback = nameof(Rollback);
    
    private static readonly Dictionary<string, string> ActionDescriptions = new()
    {
        { Access, "檢視" },
        { Insert, "新增" },
        { Modify, "編輯" },
        { Delete, "刪除" },
        { Execute, "執行" },
        { Commit, "提交" },
        { Rollback, "回復" }
    };

    public static string GetDescription(string actionName)
    {
        return ActionDescriptions.TryGetValue(actionName, out var description) ? description : actionName;
    }
}
