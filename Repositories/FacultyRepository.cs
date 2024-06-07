using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly ApplicationDbContext _context;

        public FacultyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Faculty>> GetAllFacultiesAsync()
        {
            return await _context.Faculties.ToListAsync();
        }

        public async Task<Faculty> GetFacultyByIdAsync(int id)
        {
            return await _context.Faculties.FindAsync(id);
        }

        public async Task<Faculty> GetFacultyByNameAsync(string facName)
        {
            return await _context.Faculties.FirstOrDefaultAsync(c => c.FacultyName.ToLower() == facName.ToLower());
        }

        public async Task<Faculty> AddFacultyAsync(Faculty faculty)
        {
            _context.Faculties.Add(faculty);
            await _context.SaveChangesAsync();
            return faculty;
        }

        public async Task<Faculty> UpdateFacultyAsync(Faculty faculty)
        {
            _context.Faculties.Update(faculty);
            await _context.SaveChangesAsync();
            return faculty;
        }

        public async Task<bool> DeleteFacultyAsync(int id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return false;
            }

            _context.Faculties.Remove(faculty);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
