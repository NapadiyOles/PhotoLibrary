using System;
using Microsoft.AspNetCore.Identity;
using PhotoLibrary.Data.Entities;

namespace PhotoLibrary.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        UserManager<User> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
    }
}