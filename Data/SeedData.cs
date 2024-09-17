using EmployeeManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data
{
    public static class SeedData
    {
        //public static async Task SeedRoles(IServiceProvider serviceProvider)
        //{
        //    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        //    if (!context.Sys_Roles.Any())
        //    {
        //        context.Sys_Roles.Add(new SystemRoles { RoleName = "Admin" });
        //        context.Sys_Roles.Add(new SystemRoles { RoleName = "User" });
        //        await context.SaveChangesAsync();
        //    }
        //}


        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            var rolesToSeed = new List<string>
            {
                "Admin",
                "User",
                "HR_Admin",
                "MED_Admin",
                "Faculty_Officer",
                "HOD_Admin",
                "Dean_Admin"
            };

            foreach (var roleName in rolesToSeed)
            {

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }


                if (!context.Sys_Roles.Any(r => r.RoleName == roleName))
                {
                    context.Sys_Roles.Add(new SystemRoles { RoleName = roleName });
                }
            }

            await context.SaveChangesAsync();
        }

    }



}
