using NUlid;

namespace Clean_Architecture.Domain.Entities;

public class Account : BaseAuditableEntity
{
    /// <summary>
    /// 帳號編號
    /// </summary>
    public Ulid Uuid { get; set; } = Ulid.NewUlid();

    /// <summary>
    /// 帳號編號
    /// </summary>
    public string? ApplicationUserId { get; set; }

    /// <summary>
    /// 權限群組名稱
    /// </summary>
    public string? RoleGroupId { get; set; }

    /// <summary>
    /// 啟用狀態
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 帳號名稱
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 職稱
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 分機
    /// </summary>
    public string? TelEx { get; set; }

    private Sex _sex = Sex.Male; 

    /// <summary>
    /// 性別
    /// </summary>
    public Sex Sex
    {
        get => _sex;
        set => _sex = value;
    }
    

    /// <summary>
    /// 身分證字號   
    /// </summary>
    public string? IdNumber { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 聯絡人
    /// </summary>
    public string? Contact { get; set; }

    /// <summary>
    /// 聯絡人郵遞區號
    /// </summary>
    public string? Post { get; set; }

    /// <summary>
    /// 聯絡人縣市
    /// </summary>
    public string? CityId { get; set; }

    /// <summary>
    /// 聯絡人鄉鎮
    /// </summary>
    public string? TownId { get; set; }

    /// <summary>
    /// 聯絡人地址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    public string? Telephone { get; set; }

    /// <summary>
    /// 手機
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// 傳真
    /// </summary>
    public string? Fax { get; set; }

    /// <summary>
    /// 信箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 電子郵件確認
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// 單位
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 圖片連結
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// 顯示排序
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 登入次數
    /// </summary>
    public int LoginCount { get; set; }

    /// <summary>
    ///  前三次密碼
    /// </summary>
    public string? PasswordThreeTimes { get; set; }

    /// <summary>
    /// 是否刪除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 密碼更新時間
    /// </summary>
    public DateTime? PasswordUpdateTime { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? Notes { get; set; }

    public ICollection<AccountRoleGroup> AccountRoleGroups { get; set; } = new List<AccountRoleGroup>();
}
