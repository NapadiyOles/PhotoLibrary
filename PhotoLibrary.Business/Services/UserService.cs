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
    /// <summary>
    /// Provides actions with authorized users
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Gives an info about all users
        /// </summary>
        /// <returns>List of users</returns>
        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await _db.UserManager.GetUsersInRoleAsync(RoleTypes.User);

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
        
        /// <summary>
        /// Gives an admin role to user
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <exception cref="ArgumentNullException">Throws when user was not found</exception>
        /// <exception cref="AuthenticationException">Throws when admin role is already added</exception>
        public async Task AddUserToAdminRoleAsync(string id)
        {
            User user = await _db.UserManager.FindByIdAsync(id)
                        ?? throw new ArgumentNullException(nameof(user));

            var result = await _db.UserManager.AddToRoleAsync(user, RoleTypes.Admin);

            if (!result.Succeeded)
                throw new AuthenticationException(result.Errors
                    .Select(e => new IdentityException(e.Description)));
        }

        /// <summary>
        /// Deletes admin role from user
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <exception cref="ArgumentException">Throws when user id is invalid</exception>
        /// <exception cref="ArgumentNullException">Throws when user was not found</exception>
        /// <exception cref="AuthenticationException">Throws when occured problem with deletion user from DB</exception>
        public async Task DeleteUserFromAdminRoleAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value can't be null or empty", nameof(id));

            User user = await _db.UserManager.FindByIdAsync(id)
                        ?? throw new ArgumentNullException(nameof(user));

            var result = await _db.UserManager.RemoveFromRoleAsync(user, RoleTypes.Admin);

            if (!result.Succeeded)
                throw new AuthenticationException(result.Errors
                    .Select(e => new IdentityException(e.Description)));
        }
        
        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <exception cref="ArgumentException">Throws when user id is invalid</exception>
        /// <exception cref="ArgumentNullException">Throws when user was not found</exception>
        public async Task DeleteByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value can't be null or empty", nameof(id));

            User user = await _db.UserManager.FindByIdAsync(id) ?? 
                        throw new ArgumentNullException(nameof(user));

            var pictureIds =  await _db.PictureRepository.GetIds(p => p.UserId == user.Id);

            _db.PictureRepository.DeleteMany(pictureIds);

            await _db.UserManager.DeleteAsync(user);
        }
    }
}