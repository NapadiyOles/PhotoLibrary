using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PhotoLibrary.Data.Entities;
using PhotoLibrary.Data.Interfaces;
using PhotoLibrary.Data.Repositories;

namespace PhotoLibrary.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly AppDbContext _context;
        private PictureRepository _pictureRepository;

        public UnitOfWork(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _disposed = false;
            _context = context;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public UserManager<User> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public PictureRepository PictureRepository => _pictureRepository ??= new PictureRepository(_context);

        public async Task SaveAsync() =>
            await _context.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            UserManager?.Dispose();
            RoleManager?.Dispose();
            
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}