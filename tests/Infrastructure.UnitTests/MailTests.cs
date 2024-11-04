using Moq;
using Microsoft.Extensions.Configuration;
using Clean_Architecture.Application.Common.Interfaces;
using Clean_Architecture.Infrastructure.Mail;
using NUnit.Framework;

namespace Clean_Architecture.Infrastructure.UnitTests;

public class MailTests
{
    private IMailService _mailService;
    
    [OneTimeSetUp]
    public void Setup()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(currentDirectory, "..", "..", ".."))
            .AddJsonFile("appsettings.json")
            .Build();

        _mailService = new MailService(configuration);
    }
    
    [Test]
    public void TestSendMail()
    {
        // 內網環境無法測試，請在外網環境測試
        _mailService.SendMail("your@email.com", "subject", "body");
    }
}
