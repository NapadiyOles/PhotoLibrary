using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PhotoLibrary.Data.Entities;
using PhotoLibrary.Data.Interfaces;

namespace PhotoLibrary.Data.Repositories
{
    public class PictureRepository : IRepository<Picture>
    {
        private readonly AppDbContext _context;

        public PictureRepository(AppDbContext context) => _context = context;

        public IQueryable<Picture> GetAll() => _context.Pictures.AsQueryable();

        public IQueryable<Picture> GetMany(Expression<Func<Picture, bool>> expression) =>
            _context.Pictures.Where(expression).AsQueryable();

        public async Task<Picture> GetByIdAsync(int id) => await _context.Pictures.FindAsync(id);

        public async Task AddAsync(Picture entity) => await _context.AddAsync(entity);

        public void Update(Picture entity) => _context.Update(entity);

        public void DeleteById(int id) => _context.Remove(new Picture {Id = id});
    }
}