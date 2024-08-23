using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories.CoreFeatures
{
    public class EmpPositionRepository : IEmpPositionRepository
    {
        private ApplicationDbContext _context;

        public EmpPositionRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<EmpPositions> AddPositionAsync(EmpPositions position)
        {
            _context.Emp_Positions.Add(position);

            await _context.SaveChangesAsync();

            return position;
        }

        public async Task<List<EmpPositions>> GetPositionsByEmployeeIdAsync(int employeeId)
        {
            var positions = await _context.Emp_Positions.Where(e => e.EmployeeId == employeeId).ToListAsync();

            return positions;
        }
    }
}
