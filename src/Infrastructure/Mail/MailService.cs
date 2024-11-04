using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Clean_Architecture.Application.Common.Interfaces;

namespace Clean_Architecture.Infrastructure.Mail;

public class MailService : IMailService
{
    private readonly IConfiguration _smtpConfiguration;
    private IConfigurationSection _smtpConfigurationSection;

    public MailService(IConfiguration configuration)
    {
        _smtpConfiguration = configuration;
        _smtpConfigurationSection = _smtpConfiguration.GetSection("Smtp:Default");
    }

    public void setSmtpConfiguration(string provider)
    {
        _smtpConfigurationSection = _smtpConfiguration.GetSection($"Smtp:{provider}");
    }

    public void SendMail(string to, string subject, string body)
    {
        var smtpConfiguration = _smtpConfigurationSection;

        Guard.Against.Null(smtpConfiguration, message: "Smtp configuration not found.");

        var host = smtpConfiguration.GetValue<string>("Host");
        var port = smtpConfiguration.GetValue<int>("Port");
        var userName = smtpConfiguration.GetValue<string>("UserName");
        var password = smtpConfiguration.GetValue<string>("Password");

        MailMessage mail = new MailMessage()
        {   
            From = new MailAddress(userName!), To = { new MailAddress(to) }, Subject = subject, Body = body
        };

        SmtpClient smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(
                userName,
                password
            ),
            EnableSsl = true
        };

        try
        {
            smtpClient.SendMailAsync(mail).Wait();
        }
        catch (Exception ex)
        {
            // Handle exception or log it
            throw new InvalidOperationException("SMTP configuration test failed.", ex);
        }
    }
}
