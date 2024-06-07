using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class SystemUsersRepository : ISystemUsersRepository
    {
        private readonly ApplicationDbContext _context;

        public SystemUsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(SystemUsers user)
        {
            await _context.Sys_Users.AddAsync(user);
        }

        public async Task<SystemUsers> GetUserByIdAsync(int id)
        {
            return await _context.Sys_Users.FindAsync(id);
        }

        public async Task<SystemUsers> GetUserByEmployeeIdAsync(int id)
        {
            return await _context.Sys_Users.FirstOrDefaultAsync(c => c.EmployeeId == id);
        }


        public async Task<SystemUsers> GetUserByEmail(string email)
        {
            var user = await _context.Sys_Users.FirstOrDefaultAsync( c => c.Email == email);

            return user;
        }

        public async Task<IEnumerable<SystemUsers>> GetAllUsersAsync()
        {
            return await _context.Sys_Users.ToListAsync();
        }

        public void Update(SystemUsers user)
        {
            _context.Sys_Users.Update(user);
        }

        public void Remove(SystemUsers user)
        {
            _context.Sys_Users.Remove(user);
        }
    }
}
