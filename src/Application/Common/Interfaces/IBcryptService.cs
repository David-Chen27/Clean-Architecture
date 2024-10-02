namespace Clean_Architecture.Application.Common.Interfaces;

public interface IBcryptService
{
    public string HashPassword(string password);

    public bool VerifyPassword(string password, string hashPassword);
}
