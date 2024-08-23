using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures
{
    public interface ICredentialsRepository
    {
        Task<Credentials> AddFileAsync(Credentials file);
        Task<IEnumerable<Credentials>> GetUploadsByEmployeeIdAsync(int employeeId);

        Task<IEnumerable<Credentials>> GetUploadsAsync();
    }
}
