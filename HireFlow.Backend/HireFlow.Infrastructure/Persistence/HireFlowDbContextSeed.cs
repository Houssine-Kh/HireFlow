using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Domain.Users.Entities;
using HireFlow.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HireFlow.Infrastructure.Persistence
{
    public class HireFlowDbContextSeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<HireFlowDbContextSeed>>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            await SeedRoles(roleManager, logger);
            await SeedAdmin(userManager, logger);
        }
        public static async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager, ILogger logger)
        {
            string[] roles = { "Admin", "Candidate", "Recruiter" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    logger.LogInformation($"Created role : {role}");
                }
            }
        }

        public static async Task SeedAdmin(UserManager<AppUser> userManager, ILogger logger)
        {
            var adminEmail = "admin@hireflow.com";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new AppUser
                {
                    FirstName = "System",
                    LastName = "Admin",
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (!result.Succeeded)
                {
                    logger.LogError("Failed to create admin user.");
                    return;
                }
            }
            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                await userManager.AddToRoleAsync(admin, "Admin");
                logger.LogInformation("Admin added to role Admin.");
            }

        }
    }
}