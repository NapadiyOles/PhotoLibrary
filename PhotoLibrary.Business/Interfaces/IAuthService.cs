using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Business.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserDTO model);
        Task<SecurityToken> LogInAsync(UserDTO model);
    }
}