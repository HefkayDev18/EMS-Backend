using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures;
using Microsoft.EntityFrameworkCore;
using Sprache;

namespace EmployeeManagementSystem.Repositories.CoreFeatures
{
    public class EhHistoryRepository : IEhHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EhHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EmpHistory>> GetEmploymentHistoryByEmployeeIdAsync(int employeeId)
        {
            return await _context.Emp_History
                .Where(eh => eh.EmployeeId == employeeId)
                .OrderByDescending(eh => eh.Id)
                .ToListAsync();
        }

        public async Task<EmpHistory> GetEmpHistoryByIdAsync(int employeeId)
        {
            var empHistory = await _context.Emp_History
                .SingleOrDefaultAsync(eh => eh.EmployeeId == employeeId);

            return empHistory;
        }

        public async Task<EmpHistory> AddEmployeeHistoryAsync(EmpHistory empHistory)
        {
            _context.Emp_History.Add(empHistory);

            await _context.SaveChangesAsync();

            return empHistory;
        }

        public async Task<EmpHistory> UpdateEmpHistoryAsync(EmpHistory empHistory)
        {
            _context.Entry(empHistory).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return empHistory;
        }

        public async Task<IEnumerable<EmpHistory>> GetEmploymentHistoriesByEmployeeIdAsync(int employeeId)
        {
            return await _context.Emp_History
                .Where(eh => eh.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}
