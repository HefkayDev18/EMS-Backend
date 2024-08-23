using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.CoreFeatures;
using EmployeeManagementSystem.Repositories.Interfaces;
using EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures;

namespace EmployeeManagementSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Employees = new EmployeeRepository(_context);
            Sys_Users = new SystemUsersRepository(_context);
            Sys_Roles = new SystemRolesRepository(_context);
            Departments = new DepartmentRepository(_context);
            Faculties = new FacultyRepository(_context);
            Emp_History = new EhHistoryRepository(_context);
            Emp_Positions = new EmpPositionRepository(_context);
            Emp_Appraisals = new EmpAppraisalRepository(_context);
            Emp_MedRecords = new EmpMedRecordsRepository(_context);
            Emp_Credentials = new CredentialsRepository(_context);
            //Emp_AppraisalsComment = new EmpAppraisalCommentRepository(_context);
        }

        public IEmployeeRepository Employees { get; private set; }
        public ISystemUsersRepository Sys_Users { get; private set; }
        public ISystemRolesRepository Sys_Roles { get; private set; }
        public IDepartmentRepository Departments { get; private set; }
        public IFacultyRepository Faculties { get; private set; }
        public IEhHistoryRepository Emp_History { get; private set; }
        public IEmpPositionRepository Emp_Positions { get; private set; }
        public IEmpAppraisalRepository Emp_Appraisals { get; private set; }
        public IEmpMedRecordsRepository Emp_MedRecords { get; private set; }
        public ICredentialsRepository Emp_Credentials { get; private set; }
        //public IEmpAppraisalCommentRepository Emp_AppraisalsComment { get; private set; }


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
