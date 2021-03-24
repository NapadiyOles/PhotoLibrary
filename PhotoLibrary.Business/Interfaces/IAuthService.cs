using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Business.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO model);
        Task<SecurityToken> LogInAsync(LogInDTO model);
    }
}