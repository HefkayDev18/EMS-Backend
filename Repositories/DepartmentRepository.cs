using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department> GetDepartmentByNameAsync(string deptName)
        {
            return await _context.Departments.FirstOrDefaultAsync(c => c.DepartmentName.ToLower() == deptName.ToLower());
        }

        public async Task<Department> AddDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return false;
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
