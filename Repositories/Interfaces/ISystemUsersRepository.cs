using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface ISystemUsersRepository
    {
        Task AddUserAsync(SystemUsers user);
        Task<SystemUsers> GetUserByIdAsync(int id);
        Task<SystemUsers> GetUserByEmployeeIdAsync(int id);
        Task<SystemUsers> GetUserByEmail(string email);
        Task<IEnumerable<SystemUsers>> GetAllUsersAsync();
        void Update(SystemUsers user);
        void Remove(SystemUsers user);
    }
}
