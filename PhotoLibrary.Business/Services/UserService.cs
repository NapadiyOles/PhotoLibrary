using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PhotoLibrary.Business.Exceptions;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;
using PhotoLibrary.Data.Entities;
using PhotoLibrary.Data.Interfaces;

namespace PhotoLibrary.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await _db.UserManager.GetUsersInRoleAsync(RoleTypes.User);

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
        
        public async Task AddUserToAdminRoleAsync(string id)
        {
            User user = await _db.UserManager.FindByIdAsync(id)
                        ?? throw new ArgumentNullException(nameof(user));

            var result = await _db.UserManager.AddToRoleAsync(user, RoleTypes.Admin);

            if (!result.Succeeded)
                throw new AuthenticationException(result.Errors
                    .Select(e => new IdentityException(e.Description)));
        }

        public async Task DeleteUserFromAdminRoleAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Value can't be null or empty", nameof(id));

            User user = await _db.UserManager.FindByIdAsync(id)
                        ?? throw new ArgumentNullException(nameof(user));

            var result = await _db.UserManager.RemoveFromRoleAsync(user, RoleTypes.Admin);

            if (!result.Succeeded)
                throw new AuthenticationException(result.Errors
                    .Select(e => new IdentityException(e.Description)));
        }
        
        public async Task DeleteByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Value can't be null or empty", nameof(id));

            User user = await _db.UserManager.FindByIdAsync(id) ?? 
                        throw new ArgumentNullException(nameof(user));

            var pictureIds =  await _db.PictureRepository.GetIds(p => p.UserId == user.Id);

            _db.PictureRepository.DeleteMany(pictureIds);

            await _db.UserManager.DeleteAsync(user);
        }
    }
}