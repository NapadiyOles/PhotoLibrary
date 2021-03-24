using System.Linq;
using System.Threading.Tasks;
using PhotoLibrary.Data.Entities;

namespace PhotoLibrary.Data.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();

        Task<T> GetByIdAsync(int id);
        
        Task AddAsync(T entity);
        
        Task UpdateAsync(T entity);
        
        Task DeleteByIdAsync(int id);
    }
}