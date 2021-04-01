using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PhotoLibrary.Data.Entities;

namespace PhotoLibrary.Data.Helpers
{
    public class IdentityDataSeeder
    {
        public static async Task SeedIdentityData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new() {Name = "admin"});
                await roleManager.CreateAsync(new() {Name = "user"});
            }

            if (!(await userManager.GetUsersInRoleAsync("admin")).Any())
            {
                var user = new User {UserName = "Admin"};

                await userManager.CreateAsync(user, "DefaultAdmin_001");

                await userManager.AddToRolesAsync(user, new[] {"user", "admin"});
            }
        }
    }
}