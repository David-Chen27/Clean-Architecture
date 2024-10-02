using Clean_Architecture.Application.Common.Interfaces;

namespace Clean_Architecture.Infrastructure.Bcrypt;

public class BcryptService : IBcryptService
{
    /// <summary>
    /// Hash password with BCrypt
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verify password with BCrypt
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hashedPassword"></param>
    /// <returns></returns>
    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
