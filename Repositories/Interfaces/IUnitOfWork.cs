using EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures;

namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository Employees { get; }
        ISystemUsersRepository Sys_Users { get; }
        ISystemRolesRepository Sys_Roles { get; }
        IDepartmentRepository Departments { get; }
        IFacultyRepository Faculties { get; }
        IEhHistoryRepository Emp_History { get; }
        IEmpPositionRepository Emp_Positions { get; }
        IEmpAppraisalRepository Emp_Appraisals { get; }
        IEmpMedRecordsRepository Emp_MedRecords { get; }
        ICredentialsRepository Emp_Credentials { get; }
        //IEmpAppraisalCommentRepository Emp_AppraisalsComment { get; }
        Task<int> CompleteAsync();
    }
}
