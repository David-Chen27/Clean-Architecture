using Microsoft.AspNetCore.Identity;
using Clean_Architecture.Infrastructure.Identity;
using Clean_Architecture.Application.Common.Interfaces;

namespace Clean_Architecture.Infrastructure.Mail;

public class EmailSenderService : IEmailSender<ApplicationUser>
{
    private readonly IMailService _mailService;

    public EmailSenderService(IMailService mailService)
    {
        _mailService = mailService;
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var subject = "Confirmation Link";
        var body = $"Please confirm your account by clicking this link: {confirmationLink}";
        _mailService.SendMail(email, subject, body);
        return Task.CompletedTask;
    }

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var subject = "Password Reset Link";
        var body = $"You can reset your password by clicking this link: {resetLink}";
        _mailService.SendMail(email, subject, body);
        return Task.CompletedTask;
    }

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var subject = "Password Reset Code";
        var body = $"Your password reset code is: {resetCode}";
        _mailService.SendMail(email, subject, body);
        return Task.CompletedTask;
    }
}
