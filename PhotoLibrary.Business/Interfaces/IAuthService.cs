using System.Threading.Tasks;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Business.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserDTO model);
        Task<LibraryToken> LogInAsync(UserDTO model);
    }
}