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

        public async Task RegisterAsync(UserDTO model)
        {
            var user = _mapper.Map<User>(model);

            var result = await _db.UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new AuthenticationException(result.Errors
                    .Select(er => $"Code: {er.Code}; Description: {er.Description}")
                    .Aggregate(new StringBuilder(), (curr, next) => 
                        curr.Append(next).Append('\n')).ToString());
        }

        public async Task<SecurityToken> LogInAsync(UserDTO model)
        {
            var user = await _db.UserManager.FindByNameAsync(model.Name) ?? throw new UnauthorizedException();

            if (!await _db.UserManager.CheckPasswordAsync(user, model.Password))
                throw new AuthenticationException("Password is not correct");

            var userRoles = await _db.UserManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
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

            return token;
        }
    }
}