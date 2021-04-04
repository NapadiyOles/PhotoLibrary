using System.ComponentModel.DataAnnotations;

namespace PhotoLibrary.Api.Models.User
{
    public class UserRegisterModel
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}