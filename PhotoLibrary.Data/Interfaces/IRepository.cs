using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PhotoLibrary.Data.Entities;

namespace PhotoLibrary.Data.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();

        IQueryable<Picture> GetMany(Expression<Func<Picture, bool>> expression);
        
        Task<T> GetByIdAsync(int id);
        
        Task AddAsync(T entity);
        
        void Update(T entity);
        
        void DeleteById(int id);
    }
}