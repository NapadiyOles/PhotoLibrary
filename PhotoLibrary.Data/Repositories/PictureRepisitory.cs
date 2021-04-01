using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoLibrary.Data.Entities;
using PhotoLibrary.Data.Interfaces;

namespace PhotoLibrary.Data.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly AppDbContext _context;
        private readonly string _directoryPath;

        public PictureRepository(AppDbContext context)
        {
            _context = context;

            var fullPath = AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            var projectPath = new StringBuilder();

            foreach (var folder in fullPath)
            {
                projectPath.Append($@"{folder}\");
                if(folder == "PhotoLibrary") break;
            }

            projectPath.Append($@"PhotoLibrary.Data\Pictures");

            _directoryPath = projectPath.ToString();
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
            _context.Remove(id);
        }

        private IQueryable<Picture> GetAll() => _context.Pictures.AsQueryable();

        private void SaveImage(Image image, string uniqueId) => 
            image.Save($@"{_directoryPath}\{uniqueId}.jpg", ImageFormat.Jpeg);

        private Image LoadImage(string uniqueId) => new Bitmap($@"{_directoryPath}\{uniqueId}.jpg");

        private void DeleteImage(string uniqueId) => File.Delete($@"{_directoryPath}\{uniqueId}.jpg");
    }
}