namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface INotificationService
    {
        Task NotifyAsync(string email, string subject, string message);
    }
}
