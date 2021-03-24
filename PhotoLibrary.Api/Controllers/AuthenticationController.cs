using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhotoLibrary.Api.Models;
using PhotoLibrary.Business.Exceptions;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;
using AuthenticationException = PhotoLibrary.Business.Exceptions.AuthenticationException;

namespace PhotoLibrary.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthenticationController(IAuthService service) => _service = service;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterModel model)
        {
            try
            {
                await _service.RegisterAsync(new RegisterDTO()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                });
            }
            catch (AuthenticationException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LogIn(UserLoginModel model)
        {
            SecurityToken token;

            try
            {
                token = await _service.LogInAsync(new LogInDTO
                {
                    Name = model.Name, Password = model.Password
                });
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
            catch (AuthenticationException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpGet("hi")]
        [Authorize]
        public ActionResult MakeHi()
        {
            var user = User;
            
                return Ok("hi!");
            
            // return Forbid();
        }

        [AllowAnonymous]
        [HttpGet("non-auth_hi")]
        public ActionResult SayHi()
        {
            return Ok("non auth hi");
        }
    }
}