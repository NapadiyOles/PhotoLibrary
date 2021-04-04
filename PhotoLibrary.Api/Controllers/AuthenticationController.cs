using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoLibrary.Api.Filters;
using PhotoLibrary.Api.Models.User;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthenticationExceptionFilter]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IMapper _mapper;
        public AuthenticationController(IAuthService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterModel model)
        {
            await _service.RegisterAsync(_mapper.Map<UserDTO>(model));
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> LogIn(UserLoginModel model)
        {
            var token = await _service.LogInAsync(_mapper.Map<UserDTO>(model));
            return Ok(token);
        }
    }
}