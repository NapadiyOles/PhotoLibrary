using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PhotoLibrary.Data.Entities;

namespace PhotoLibrary.Data.Interfaces
{
    public interface IPictureRepository
    {
        Task<IEnumerable<Picture>> GetAllAsync();
        Task<IEnumerable<Picture>> GetManyAsync(Expression<Func<Picture, bool>> expression);
        Task<Picture> GetByIdAsync(int id);
        Task AddAsync(Picture entity);
        void Update(Picture entity);
        void Delete(int id, string uniqueId);
    }
}