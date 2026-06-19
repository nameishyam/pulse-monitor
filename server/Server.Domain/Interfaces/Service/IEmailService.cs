namespace Server.Domain.Interfaces.Service;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}