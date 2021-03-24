using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;
using PhotoLibrary.Data.Interfaces;

namespace PhotoLibrary.Business.Services
{
    public class UserService
    {
        public readonly IUnitOfWork _db;
        public readonly IMapper _mapper;

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
        
        public async Task AddAsync(UserDTO model)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateAsync(UserDTO model)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteByIdAsync(UserDTO model)
        {
            throw new NotImplementedException();
        }
    }
}