using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Data
{
    public static class SeedData
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Sys_Roles.Any())
            {
                context.Sys_Roles.Add(new SystemRoles { RoleName = "Admin" });
                context.Sys_Roles.Add(new SystemRoles { RoleName = "User" });
                await context.SaveChangesAsync();
            }
        }
    }
}
