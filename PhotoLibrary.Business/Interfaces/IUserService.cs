using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Business.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task AddUserToAdminRoleAsync(string id);
        Task DeleteUserFromAdminRoleAsync(string id);
        Task DeleteByIdAsync(string id);
    }
}