using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PhotoLibrary.Data.Entities
{
    public class User : IdentityUser
    {
        public IEnumerable<Picture> Pictures { get; set; }
    }
}