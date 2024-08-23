using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var employee = await _context.Employees
                                            .OrderByDescending(e => e.DateCreated)
                                            .ToListAsync();

            return employee;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            return employee;
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            var employee =  await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);

            return employee;
        }


        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
