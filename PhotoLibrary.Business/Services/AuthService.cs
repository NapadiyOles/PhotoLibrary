using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PhotoLibrary.Business.Exceptions;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;
using PhotoLibrary.Data.Entities;
using PhotoLibrary.Data.Interfaces;
using AuthenticationException = PhotoLibrary.Business.Exceptions.AuthenticationException;

namespace PhotoLibrary.Business.Services
{
    /// <summary>
    /// Provides user authentication
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _db;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork db, IMapper mapper, IConfiguration config)
        {
            _db = db;
            _mapper = mapper;
            _config = config;
        }

        /// <summary>
        /// Adds new users to database
        /// </summary>
        /// <param name="model">Contains user credentials</param>
        /// <exception cref="AuthenticationException">Throws when registration is failed</exception>
        public async Task RegisterAsync(UserDTO model)
        {
            var user = _mapper.Map<User>(model);
            
            var result = await _db.UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new AuthenticationException(result.Errors
                    .Select(e => new IdentityException(e.Description)));

            await _db.UserManager.AddToRoleAsync(user, RoleTypes.User);
        }

        /// <summary>
        /// Gives an authorization token for preregistered users
        /// </summary>
        /// <param name="model">Contains user credentials</param>
        /// <returns>Authorization token</returns>
        /// <exception cref="UnregisteredException">Throws when user in not registered</exception>
        /// <exception cref="AuthenticationException">Throws when user credentials is invalid</exception>
        public async Task<LibraryToken> LogInAsync(UserDTO model)
        {
            var user = await _db.UserManager.FindByNameAsync(model.Name) ?? throw new UnregisteredException();

            if (!await _db.UserManager.CheckPasswordAsync(user, model.Password))
                throw new AuthenticationException("Password is not correct");

            var userRoles = await _db.UserManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Id),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

            var tokenHandler = new JwtSecurityTokenHandler();
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(authSignInKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LibraryToken
                {Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo};
        }
    }
}