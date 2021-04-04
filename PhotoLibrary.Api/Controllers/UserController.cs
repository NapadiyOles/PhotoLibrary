using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    [UserExceptionFilter]
    [Authorize(Roles = RoleTypes.Admin)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public UserController(IUserService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers()
        {
            var users = await _service.GetUsersAsync();

            if (!users.Any()) return NotFound();

            return Ok(_mapper.Map<IEnumerable<UserViewModel>>(users));
        }

        [HttpPut("create_admin")]
        public async Task<ActionResult> CreateAdmin([Required] string id)
        {
            await _service.AddUserToAdminRoleAsync(id);
            return Ok();
        }
        
        [HttpPut("delete_admin")]
        public async Task<ActionResult> DeleteAdmin([Required] string id)
        {
            await _service.DeleteUserFromAdminRoleAsync(id);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser([Required] string id)
        {
            await _service.DeleteByIdAsync(id);
            return Ok();
        }
    }
}
