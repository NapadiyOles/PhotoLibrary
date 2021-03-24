using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoLibrary.Business.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task AddAsync(T model);

        Task UpdateAsync(T model);

        Task DeleteByIdAsync(int modelId);
    }
}