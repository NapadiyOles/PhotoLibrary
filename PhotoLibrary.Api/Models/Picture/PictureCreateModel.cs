using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PhotoLibrary.Api.Models.Picture
{
    public class PictureCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}