using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Clean_Architecture.Infrastructure.Encryption;
using NUnit.Framework;

namespace Clean_Architecture.Infrastructure.UnitTests;

public class EncryptionServiceTests
{
    private EncryptionService _encryptionService;

    [OneTimeSetUp]
    public void Setup()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(currentDirectory, "..", "..", ".."))
            .AddJsonFile("appsettings.json")
            .Build();

        _encryptionService = new EncryptionService(configuration);
    }

    [Test]
    public void shouldEncryptAndDecrypt()
    {
        var plainText = "Hello World";
        var encrypted = _encryptionService.Encrypt(plainText);
        var decrypted = _encryptionService.Decrypt(encrypted);
        decrypted.Should().Be(plainText);
    }
}
