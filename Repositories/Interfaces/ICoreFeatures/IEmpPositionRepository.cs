using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures
{
    public interface IEmpPositionRepository
    {
        Task<EmpPositions> AddPositionAsync(EmpPositions position);
        Task<List<EmpPositions>> GetPositionsByEmployeeIdAsync(int employeeId);
    }
}
