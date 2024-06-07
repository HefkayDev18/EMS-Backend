using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<Department> GetDepartmentByNameAsync(string deptName);
        Task<Department> AddDepartmentAsync(Department department);
        Task<Department> UpdateDepartmentAsync(Department department);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
