namespace Server.Service.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}