using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Business.Interfaces
{
    public interface IPictureService
    {
        Task<IEnumerable<PictureDTO>> GetAllAsync();
        Task<IEnumerable<PictureDTO>> GetAllByUserIdAsync(string userId);
        Task<Image> GetImageByIdAsync(int id);
        Task AddAsync(PictureDTO model);
        Task ChangeNameAsync(int id, string name, string userId);
        Task RateAsync(int id, double rate);
        Task DeleteByIdAsync(PictureDTO model);
    }
}