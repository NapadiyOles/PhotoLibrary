using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PhotoLibrary.Data.Entities;
using PhotoLibrary.Data.Repositories;

namespace PhotoLibrary.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        UserManager<User> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        PictureRepository PictureRepository { get; }
        Task SaveAsync();
    }
}