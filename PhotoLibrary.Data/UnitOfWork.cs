using Microsoft.AspNetCore.Identity;
using PhotoLibrary.Data.Entities;
using PhotoLibrary.Data.Interfaces;

namespace PhotoLibrary.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        private UserManager<User> _userManager;

        public UnitOfWork(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context; UserManager = userManager; RoleManager = roleManager;
        }

        public UserManager<User> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }

        protected virtual void Dispose(bool disposing)
        {
            
        }

        public void Dispose()
        {
            _userManager?.Dispose();
        }
    }
}