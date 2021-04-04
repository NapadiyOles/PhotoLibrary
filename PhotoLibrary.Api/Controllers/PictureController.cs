using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoLibrary.Api.Filters;
using PhotoLibrary.Api.Models.Picture;
using PhotoLibrary.Business.Exceptions;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [PictureExceptionFilter]
    [Authorize]
    public class PictureController : ControllerBase
    {
        private readonly IPictureService _service;
        private readonly IMapper _mapper;

        public PictureController(IPictureService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PictureViewModel>>> GetAll()
        {
            var pictures = await _service.GetAllAsync();

            if (!pictures.Any()) return NotFound();

            return Ok(_mapper.Map<IEnumerable<PictureViewModel>>(pictures));
        }
        
        [HttpGet("by_current_user")]
        public async Task<ActionResult<IEnumerable<PictureViewModel>>> GetAllByUser()
        {
            var userId = User.Claims.ElementAt(0).Value;

            var pictures = await _service.GetAllByUserIdAsync(userId);

            if (!pictures.Any()) return NotFound();

            return Ok(_mapper.Map<IEnumerable<PictureViewModel>>(pictures));
        }

        [HttpGet("{id}"), DisableRequestSizeLimit]
        public async Task<ActionResult> GetImage(int id)
        {
            byte[] imageContent;

            var image = await _service.GetImageByIdAsync(id);

            using (var stream = new  MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg);
                imageContent = stream.ToArray();
            }

            return File(imageContent, "image/jpeg");
        }
        
        [HttpPost]
        public async Task<ActionResult> AddPicture([FromForm] PictureCreateModel model)
        {
            var userId = User.Claims.ElementAt(0).Value;

            using (var stream = new MemoryStream())
            {
                await model.Image.CopyToAsync(stream);
                
                await _service.AddAsync(new PictureDTO
                    {Name = model.Name, Image = new Bitmap(stream), UserId = userId}
                );
            }
            
            return Ok();
        }

        [HttpPut("name/{id}")]
        public async Task<ActionResult> ChangeName(int id, [Required]string name)
        {
            var userId = User.Claims.ElementAt(0).Value;

            await _service.ChangeNameAsync(id, name, userId);
            return Ok();
        }

        [HttpPut("rate/{id}")]
        public async Task<ActionResult> Rate(int id, [Required]double rate)
        {
            await _service.RateAsync(id, rate);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [Required] string uniqueId)
        {
            await _service.DeleteByIdAsync(new PictureDTO {Id = id, UniqueId = uniqueId});
            return Ok();
        }
    }
}