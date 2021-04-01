using NUnit.Framework;
using PhotoLibrary.Data;
using PhotoLibrary.Data.Repositories;

namespace PhotoLibrary.Tests
{
    public class Test
    {
        private AppDbContext _context;
        private PictureRepository _repository;

        [Test]
        public void GetRepository()
        {
            _repository = new PictureRepository(_context);
        }

    }
}