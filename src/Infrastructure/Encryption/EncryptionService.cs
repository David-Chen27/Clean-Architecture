using Clean_Architecture.Application.Common.Interfaces;

namespace Clean_Architecture.Infrastructure.Encryption;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

public class EncryptionService : IEncryptionService
{
    private readonly string _appKey;

    public EncryptionService(IConfiguration configuration)
    {
        _appKey = configuration["AppKey"] ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        var key = Encoding.UTF8.GetBytes(_appKey);
        Array.Resize(ref key, aes.Key.Length);
        aes.Key = key;

        var iv = aes.IV;
        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        var encrypted = ms.ToArray();
        var result = new byte[iv.Length + encrypted.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipherText)
    {
        var fullCipher = Convert.FromBase64String(cipherText);
        using var aes = Aes.Create();
        var iv = new byte[aes.IV.Length];
        var cipher = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        var key = Encoding.UTF8.GetBytes(_appKey);
        Array.Resize(ref key, aes.Key.Length);
        aes.Key = key;

        using var decryptor = aes.CreateDecryptor(aes.Key, iv);
        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}
