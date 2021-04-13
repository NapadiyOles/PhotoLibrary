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
    /// <summary>
    /// Provides crud actions for picture data
    /// </summary>
    public class PictureService : IPictureService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        public PictureService(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// Gives an info about pictures of all users
        /// </summary>
        /// <returns>List of picture data</returns>
        public async Task<IEnumerable<PictureDTO>> GetAllAsync()
        {
            var pictures = await _db.PictureRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<PictureDTO>>(pictures);
        }

        /// <summary>
        /// Gives an info about pictures of current user
        /// </summary>
        /// <param name="userId">Guid of current user</param>
        /// <returns>List of picture data</returns>
        /// <exception cref="ArgumentException">Throws when user id is invalid</exception>
        public async Task<IEnumerable<PictureDTO>> GetAllByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("Value can't be null or empty", nameof(userId));

            var pictures = await _db.PictureRepository
                .GetManyAsync(p => p.UserId == userId);

            return _mapper.Map<IEnumerable<PictureDTO>>(pictures);
        }

        /// <summary>
        /// Gives picture by id
        /// </summary>
        /// <param name="id">Id of the picture</param>
        /// <returns>Picture file</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when id is invalid</exception>
        public async Task<Image> GetImageByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive");

            var picture = await _db.PictureRepository.GetByIdAsync(id);
            return picture.Image;
        }
        
        /// <summary>
        /// Adds new picture
        /// </summary>
        /// <param name="model">Picture data</param>
        /// <exception cref="ArgumentNullException">Throws when model is null</exception>
        public async Task AddAsync(PictureDTO model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model), "Instance can't be null");

            var picture = _mapper.Map<Picture>(model);
            picture.UniqueId = Guid.NewGuid().ToString();
            await _db.PictureRepository.AddAsync(picture);
            await _db.SaveAsync();
        }

        /// <summary>
        /// Changes name of picture
        /// </summary>
        /// <param name="id">Picture id</param>
        /// <param name="name">New picture name</param>
        /// <param name="userId">User guid</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when picture id is invalid</exception>
        /// <exception cref="ArgumentException">Throws when picture name is invalid</exception>
        /// <exception cref="IdentityException">Throws when user is not an author</exception>
        public async Task ChangeNameAsync(int id, string name, string userId)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Value must be positive");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value can't be null or empty", nameof(name));

            var picture = await _db.PictureRepository.GetByIdAsync(id);

            if (picture.UserId == userId) throw new IdentityException("Only picture author can change the name");
            
            picture.Name = name;
            _db.PictureRepository.Update(picture);
            await _db.SaveAsync();
        }

        /// <summary>
        /// Rates picture
        /// </summary>
        /// <param name="id">Picture id</param>
        /// <param name="rate">Picture rate value</param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when id or rate value are invalid</exception>
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
        
        /// <summary>
        /// Deletes picture
        /// </summary>
        /// <param name="model">Picture</param>
        /// <exception cref="ArgumentNullException">Throws when model is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws when id field is invalid</exception>
        /// <exception cref="ArgumentException">Throws when unique id field is invalid</exception>
        public async Task DeleteByIdAsync(PictureDTO model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model), "Instance can't be null");

            if (model.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(model), "Value of id field must be positive");

            if (string.IsNullOrWhiteSpace(model.UniqueId))
                throw new ArgumentException("Value of uniqueId field can't be null or empty", nameof(model));

            _db.PictureRepository.Delete(model.Id, model.UniqueId);
            await _db.SaveAsync();
        }
    }
}