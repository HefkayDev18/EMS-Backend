using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories
{
    public class SystemRolesRepository : ISystemRolesRepository
    {
        private readonly ApplicationDbContext _context;

        public SystemRolesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SystemRoles> GetRoleByIdAsync(int roleId)
        {
            return await _context.Sys_Roles.FindAsync(roleId);
        }

        public async Task<SystemRoles> GetRoleByNameAsync(string roleName)
        {
            return await _context.Sys_Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<IEnumerable<SystemRoles>> GetAllRolesAsync()
        {
            return await _context.Sys_Roles.ToListAsync();
        }

        public async Task AddRoleAsync(SystemRoles role)
        {
            await _context.Sys_Roles.AddAsync(role);
        }

        public async Task UpdateRoleAsync(SystemRoles role)
        {
            _context.Sys_Roles.Update(role);
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _context.Sys_Roles.FindAsync(roleId);
            if (role != null)
            {
                _context.Sys_Roles.Remove(role);
            }
        }
    }
}
