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
        
        public async Task<IEnumerable<UserDTO>> GetAllByRole(string type)
        {
            var users = await _db.UserManager.GetUsersInRoleAsync(type);

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task CreateAdmin(UserDTO model)
        {
            var user = _mapper.Map<User>(model);
            var result = await _db.UserManager.CreateAsync(user);
            
            if (!result.Succeeded)
                throw new AuthenticationException(result.Errors
                    .Select(e => new IdentityException(e.Description)));

            await _db.UserManager.AddToRolesAsync(user, new[] {RoleTypes.User, RoleTypes.Admin});
        }
        
        public async Task DeleteByIdAsync(UserDTO model)
        {
            var user = _mapper.Map<User>(model);
            await _db.UserManager.DeleteAsync(user);
        }
    }
}