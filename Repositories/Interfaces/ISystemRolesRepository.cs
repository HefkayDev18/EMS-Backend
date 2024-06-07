using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface ISystemRolesRepository
    {
        Task<SystemRoles> GetRoleByIdAsync(int roleId);
        Task<SystemRoles> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<SystemRoles>> GetAllRolesAsync();
        Task AddRoleAsync(SystemRoles role);
        Task UpdateRoleAsync(SystemRoles role);
        Task DeleteRoleAsync(int roleId);
    }
}
