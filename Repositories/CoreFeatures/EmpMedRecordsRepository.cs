using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories.CoreFeatures
{
    public class EmpMedRecordsRepository : IEmpMedRecordsRepository
    {
        private ApplicationDbContext _context;

        public EmpMedRecordsRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<EmpMedRecords>> GetMedRecordsAsync()
        {
            return await _context.Emp_MedRecords
                .Include(e => e.Employee)
                .OrderByDescending(m => m.DateOfRecord)
                .ToListAsync();
        }

        public async Task<EmpMedRecords> GetMedRecordByEmpIdAsync(int id)
        {
            var record = await _context.Emp_MedRecords
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.EmployeeId == id);

            return record;
        }

        public async Task<IEnumerable<EmpMedRecords>> GetMedRecordsByEmployeeIdAsync(int employeeId)
        {
            return await _context.Emp_MedRecords
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId)
                .ToListAsync();
        }


        public async Task<EmpMedRecords> GetMedRecordByRecIdAsync(int medRecordId)
        {
            var record = await _context.Emp_MedRecords
                .FirstOrDefaultAsync(a => a.MedRecordsId == medRecordId);

            return record;
        }

        public async Task<EmpMedRecords> AddMedRecordAsync(EmpMedRecords record)
        {
            _context.Emp_MedRecords.Add(record);

            await _context.SaveChangesAsync();

            return record;
        }

        public async Task<EmpMedRecords> UpdateMedRecordAsync(EmpMedRecords record)
        {
            _context.Entry(record).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return record;
        }

        public async Task<IEnumerable<EmpMedRecords>> GetPaginatedMedRecordsAsync(int page, int pageSize)
        {
            return await _context.Emp_MedRecords
                .OrderByDescending(m => m.DateOfRecord)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


    }
}
