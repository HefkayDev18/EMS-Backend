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
        public DbSet<EmpHistory> Emp_History { get; set; }
        public DbSet<EmpPositions> Emp_Positions { get; set; }
        public DbSet<EmpAppraisal> Emp_Appraisals { get; set; }
        public DbSet<EmpMedRecords> Emp_MedRecords { get; set; }
        public DbSet<Credentials> Emp_Credentials { get; set; }
        public DbSet<Advertisements> Emp_Adverts { get; set; }
        //public DbSet<EmpAppraisalComment> Emp_AppraisalsComment { get; set; }
    }
}
