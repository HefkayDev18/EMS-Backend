using EmployeeManagementSystem.Repositories.Interfaces;
using System.Net.Mail;
using System.Net;

namespace EmployeeManagementSystem.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var fromEmail = _configuration["EmailSettings:FromEmail"];

            var smtpClient = new SmtpClient
            {
                Host = _configuration["EmailSettings:SmtpHost"],
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
