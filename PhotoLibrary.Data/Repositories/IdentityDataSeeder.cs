using System.Linq;
using Microsoft.AspNetCore.Identity;
using PhotoLibrary.Data.Entities;

namespace PhotoLibrary.Data.Repositories
{
    public static class IdentityDataSeeder
    {
        public static void Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new() {Name = "admin"}).Wait();
                roleManager.CreateAsync(new() {Name = "user"}).Wait();
            }

            if (!userManager.GetUsersInRoleAsync("admin").Result.Any())
            {
                var user = new User {UserName = "Admin"};

                userManager.CreateAsync(user, "DefaultAdmin_001").Wait();
                userManager.AddToRolesAsync(user, new[] {"user", "admin"}).Wait();
            }
        }
    }
}