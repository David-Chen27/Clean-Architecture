namespace Clean_Architecture.Application.Common.Interfaces;

public interface IEncryptionService
{
    public string Encrypt(string plainText);
    
    public string Decrypt(string cipherText);
}
