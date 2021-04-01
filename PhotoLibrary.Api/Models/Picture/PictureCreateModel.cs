using Microsoft.AspNetCore.Http;

namespace PhotoLibrary.Api.Models.Picture
{
    public class PictureCreateModel
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
    }
}