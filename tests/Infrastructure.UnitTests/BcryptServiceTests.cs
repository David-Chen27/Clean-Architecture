using FluentAssertions;
using Clean_Architecture.Infrastructure.Bcrypt;
using NUnit.Framework;

namespace Clean_Architecture.Infrastructure.UnitTests;

public class BcryptServiceTests
{
    private BcryptService _bcryptService;

    [OneTimeSetUp]
    public void Setup()
    {
        _bcryptService = new BcryptService();
    }

    [Test]
    public void shouldHashAndVerify()
    {
        var password = "123456";
        var hashedPassword = _bcryptService.HashPassword(password);
        hashedPassword.Should().NotBeNullOrWhiteSpace();

        var isVerified = _bcryptService.VerifyPassword(password, hashedPassword);
        isVerified.Should().BeTrue();
    }
}
