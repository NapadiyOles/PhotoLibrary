using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoLibrary.Data.Entities;
using PhotoLibrary.Data.Interfaces;

namespace PhotoLibrary.Data.Repositories
{
    public class PictureRepository : FileActions, IPictureRepository
    {
        private readonly AppDbContext _context;

        public PictureRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Picture>> GetAllAsync()
        {
            var pictures = await GetAll().ToListAsync();
            return pictures;
        }
        
        public async Task<IEnumerable<Picture>> GetManyAsync(Expression<Func<Picture, bool>> expression)
        {
            var pictures = GetAll().Where(expression);
            return await pictures.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetIds(Expression<Func<Picture, bool>> expression)
        {
            var pictures = GetAll().Where(expression).Select(p => p.UniqueId);
            return await pictures.ToListAsync();
        }

        public async Task<Picture> GetByIdAsync(int id)
        {
            var picture = await _context.Pictures.FindAsync(id);
            picture.Image = LoadImage(picture.UniqueId);
            return picture;
        }

        public async Task AddAsync(Picture entity)
        {
            SaveImage(entity.Image, entity.UniqueId);
            await _context.AddAsync(entity);
        }

        public void Update(Picture entity) => _context.Update(entity);

        public void Delete(int id, string uniqueId)
        {
            DeleteImage(uniqueId);
            _context.Remove(new Picture {Id = id});
        }

        public void DeleteMany(IEnumerable<string> uniqueIds)
        {
            foreach (var uniqueId in uniqueIds) DeleteImage(uniqueId);
        }

        private IQueryable<Picture> GetAll() => _context.Pictures.AsQueryable();
    }
}