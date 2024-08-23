using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures
{
    public interface IEmpMedRecordsRepository
    {
        Task<IEnumerable<EmpMedRecords>> GetMedRecordsAsync();
        Task<EmpMedRecords> GetMedRecordByEmpIdAsync(int id);
        Task<IEnumerable<EmpMedRecords>> GetMedRecordsByEmployeeIdAsync(int employeeId);
        Task<EmpMedRecords> GetMedRecordByRecIdAsync(int medRecordId);
        Task<EmpMedRecords> AddMedRecordAsync(EmpMedRecords record);
        Task<EmpMedRecords> UpdateMedRecordAsync(EmpMedRecords record);
        Task<IEnumerable<EmpMedRecords>> GetPaginatedMedRecordsAsync(int page, int pageSize);
    }
}
