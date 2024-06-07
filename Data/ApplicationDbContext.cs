using EmployeeManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<SystemUsers> Sys_Users { get; set; }
        public DbSet<SystemRoles> Sys_Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
    }
}
