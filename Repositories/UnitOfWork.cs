using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;

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
        }

        public IEmployeeRepository Employees { get; private set; }
        public ISystemUsersRepository Sys_Users { get; private set; }
        public ISystemRolesRepository Sys_Roles { get; private set; }
        public IDepartmentRepository Departments { get; private set; }
        public IFacultyRepository Faculties { get; private set; }

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
