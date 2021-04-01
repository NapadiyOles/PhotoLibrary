using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoLibrary.Api.Models.Picture;
using PhotoLibrary.Business.Interfaces;
using PhotoLibrary.Business.Models;

namespace PhotoLibrary.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PictureController : ControllerBase
    {
        private IPictureService _service;

        public PictureController(IPictureService service, IMapper mapper)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PictureViewModel>>> GetAll()
        {
            var pictures = await _service.GetAllAsync();

            if (!pictures.Any()) return NotFound();

            return Ok(pictures.Select(p =>
                new PictureViewModel {Id = p.Id, Name = p.Name, UniqueId = p.UniqueId, Rate = p.Rate}
            ));
        }
        
        [HttpGet("by_current_user")]
        public async Task<ActionResult<IEnumerable<PictureViewModel>>> GetAllByUser()
        {
            IEnumerable<PictureViewModel> pictures;
            var userId = User.Claims.ElementAt(0).Value;

            try
            {
                pictures = (await _service.GetAllByUserIdAsync(userId)).Select(p =>
                    new PictureViewModel {Id = p.Id, Name = p.Name, UniqueId = p.UniqueId, Rate = p.Rate}
                );
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            if (!pictures.Any()) return NotFound();

            return Ok(pictures);
        }

        [HttpGet("{id}"), DisableRequestSizeLimit]
        public async Task<ActionResult> GetImage(int id)
        {
            Image image;
            byte[] imageContent;

            try
            {
                image = await _service.GetImageByIdAsync(id);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(e.Message);
            }

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
                
                try
                {
                    await _service.AddAsync(new PictureDTO
                        {Name = model.Name, Image = new Bitmap(stream), UserId = userId}
                    );
                }
                catch (ArgumentNullException e)
                {
                    return BadRequest(e.Message);
                }
            }
            
            return Ok();
        }

        [HttpPut("name")]
        public async Task<ActionResult> ChangeName(int id, string name)
        {
            try
            {
                await _service.ChangeNameAsync(id, name);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPut("rate")]
        public async Task<ActionResult> Rate(int id, double rate)
        {
            try
            {
                await _service.RateAsync(id, rate);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}