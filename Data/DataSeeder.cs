using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BeFit.Data
{
    public static class DataSeeder
    {
        private const string AdminRoleName = "Administrator";
        private const string AdminEmail = "admin@befit.local";
        private const string AdminPassword = "Admin123!"; // możesz potem zmienić

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. Rola
            if (!await roleManager.RoleExistsAsync(AdminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRoleName));
            }

            // 2. Użytkownik admin
            var admin = await userManager.FindByEmailAsync(AdminEmail);
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, AdminRoleName);
                }
            }
            else
            {
                // Upewniamy się, że ma rolę
                if (!await userManager.IsInRoleAsync(admin, AdminRoleName))
                {
                    await userManager.AddToRoleAsync(admin, AdminRoleName);
                }
            }
        }
    }
}
