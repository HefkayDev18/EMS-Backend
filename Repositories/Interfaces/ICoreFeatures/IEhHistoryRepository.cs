using EmployeeManagementSystem.Models.Entities;
using Sprache;

namespace EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures
{
    public interface IEhHistoryRepository
    {
        Task<IEnumerable<EmpHistory>> GetEmploymentHistoryByEmployeeIdAsync(int employeeId);
        Task<EmpHistory> AddEmployeeHistoryAsync(EmpHistory empHistory);
        Task<EmpHistory> GetEmpHistoryByIdAsync(int employeeId);
        Task<EmpHistory> UpdateEmpHistoryAsync(EmpHistory empHistory);
        Task<IEnumerable<EmpHistory>> GetEmploymentHistoriesByEmployeeIdAsync(int employeeId);
    }
}
