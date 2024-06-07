namespace EmployeeManagementSystem.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository Employees { get; }
        ISystemUsersRepository Sys_Users { get; }
        ISystemRolesRepository Sys_Roles { get; }
        IDepartmentRepository Departments { get; }
        IFacultyRepository Faculties { get; }
        Task<int> CompleteAsync();
    }
}
