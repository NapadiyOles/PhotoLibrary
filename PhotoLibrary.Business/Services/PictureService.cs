using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using AutoMapper;
using PhotoLibrary.Business.Exceptions;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;
using PhotoLibrary.Data.Entities;
using PhotoLibrary.Data.Interfaces;

namespace PhotoLibrary.Business.Services
{
    public class PictureService : IPictureService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        public PictureService(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PictureDTO>> GetAllAsync()
        {
            var pictures = await _db.PictureRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<PictureDTO>>(pictures);
        }

        public async Task<IEnumerable<PictureDTO>> GetAllByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("Value can't be null or empty", nameof(userId));

            var pictures = await _db.PictureRepository
                .GetManyAsync(p => p.UserId == userId);

            return _mapper.Map<IEnumerable<PictureDTO>>(pictures);
        }

        public async Task<Image> GetImageByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive");

            var picture = await _db.PictureRepository.GetByIdAsync(id);
            return picture.Image;
        }
        
        public async Task AddAsync(PictureDTO model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model), "Instance can't be null");

            var picture = _mapper.Map<Picture>(model);
            picture.UniqueId = Guid.NewGuid().ToString();
            await _db.PictureRepository.AddAsync(picture);
            await _db.SaveAsync();
        }

        public async Task ChangeNameAsync(int id, string name, string userId)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Value can't be null or empty", nameof(name));

            var picture = await _db.PictureRepository.GetByIdAsync(id);

            if (picture.UserId == userId) throw new IdentityException("Only picture author can change the name");
            
            picture.Name = name;
            _db.PictureRepository.Update(picture);
            await _db.SaveAsync();
        }

        public async Task RateAsync(int id, double rate)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive");

            if (rate < 0 || rate > 10)
                throw new ArgumentOutOfRangeException(nameof(rate),"Value must be between 0 and 10");

            var picture = await _db.PictureRepository.GetByIdAsync(id);


            picture.Rate = (picture.Rate * picture.RatesNumber + rate) / (picture.RatesNumber + 1);
            picture.RatesNumber++;

            _db.PictureRepository.Update(picture);
            await _db.SaveAsync();
        }
        public async Task DeleteByIdAsync(PictureDTO model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model), "Instance can't be null");

            if (model.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(model), "Value of id field must be positive");

            if (string.IsNullOrEmpty(model.UniqueId))
                throw new ArgumentException("Value of uniqueId field can't be null or empty", nameof(model));

            _db.PictureRepository.Delete(model.Id, model.UniqueId);
            await _db.SaveAsync();
        }
    }
}