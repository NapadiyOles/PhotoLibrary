using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoLibrary.Api.Models.User;
using PhotoLibrary.Business.Exceptions;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;
using AuthenticationException = PhotoLibrary.Business.Exceptions.AuthenticationException;

namespace PhotoLibrary.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthenticationController(IAuthService service) => _service = service;

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterModel model)
        {
            try
            {
                await _service.RegisterAsync(new UserDTO
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                });
            }
            catch (AuthenticationException e)
            {
                List<string> exceptions = new List<string>();
                foreach (var exception in e.InnerExceptions)
                    exceptions.Add(exception.Message);

                return BadRequest(exceptions);
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LogIn(UserLoginModel model)
        {
            LibraryToken token;
            try 
            {
                token = await _service.LogInAsync(new UserDTO
                {
                    Name = model.Name, Password = model.Password
                });
            }
            catch (UnregisteredException)
            {
                return Unauthorized();
            }
            catch (AuthenticationException e)
            {
                return BadRequest(e.Message);
            }
            
            return Ok(token);
        }
    }
}