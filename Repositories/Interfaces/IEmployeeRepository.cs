using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Models.ViewModels.EmployeeVMs;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        //Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<ViewEmpVM>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> GetEmployeeByEmailAsync(string email);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
