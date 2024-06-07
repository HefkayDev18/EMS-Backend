using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface IFacultyRepository
    {
        Task<IEnumerable<Faculty>> GetAllFacultiesAsync();
        Task<Faculty> GetFacultyByIdAsync(int id);
        Task<Faculty> GetFacultyByNameAsync(string facName);
        Task<Faculty> AddFacultyAsync(Faculty faculty);
        Task<Faculty> UpdateFacultyAsync(Faculty faculty);
        Task<bool> DeleteFacultyAsync(int id);
    }
}
