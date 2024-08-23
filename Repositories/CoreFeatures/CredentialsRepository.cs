using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories.CoreFeatures
{
    public class CredentialsRepository : ICredentialsRepository
    {
        private readonly ApplicationDbContext _context;

        public CredentialsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Credentials> AddFileAsync(Credentials file)
        {
            _context.Emp_Credentials.Add(file);

            await _context.SaveChangesAsync();

            return file;
        }

        public async Task<IEnumerable<Credentials>> GetUploadsByEmployeeIdAsync(int employeeId)
        {
            return await _context.Emp_Credentials
                .Include(a => a.EmployeeCred)
                .Where(a => a.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Credentials>> GetUploadsAsync()
        {
            var uploads =  await _context.Emp_Credentials
                .Include(e => e.EmployeeCred)
                .OrderByDescending(e => e.UploadedDate)
                .ToListAsync();

            return uploads;
        }

    }
}
