using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Business.Services
{
    public class PictureService : IService<PictureDTO>
    {
        public async Task<IEnumerable<PictureDTO>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<PictureDTO> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddAsync(PictureDTO model)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateAsync(PictureDTO model)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            throw new System.NotImplementedException();
        }
    }
}