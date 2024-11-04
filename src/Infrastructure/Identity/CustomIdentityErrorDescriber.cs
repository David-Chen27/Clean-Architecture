using Microsoft.AspNetCore.Identity;

namespace Clean_Architecture.Infrastructure.Identity;

public class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
        public override IdentityError PasswordTooShort(int length)
    {
        return new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = $"密碼長度至少要 {length} 個字元。"
        };
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = "密碼必須包含至少一個非字母數字字符。"
        };
    }

    public override IdentityError PasswordRequiresLower()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresLower),
            Description = "密碼必須包含至少一個小寫字母（'a'-'z'）。"
        };
    }

    public override IdentityError PasswordRequiresUpper()
    {
        return new IdentityError
        {
            Code = nameof(PasswordRequiresUpper),
            Description = "密碼必須包含至少一個大寫字母（'A'-'Z'）。"
        };
    }
}
