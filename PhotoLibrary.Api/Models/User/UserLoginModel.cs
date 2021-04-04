using System.ComponentModel.DataAnnotations;

namespace PhotoLibrary.Api.Models.User
{
    public class UserLoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}