using EmployeeManagementSystem.Repositories.Interfaces;

namespace EmployeeManagementSystem.Repositories
{
    public class NotificationService(IEmailService emailService)
    {
        private readonly IEmailService _emailService = emailService;

        public async Task NotifyAsync(string email, string subject, string message)
        {
            await _emailService.SendEmailAsync(email, subject, message);
        }
    }
}
