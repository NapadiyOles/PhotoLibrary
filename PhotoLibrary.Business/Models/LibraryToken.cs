using System;

namespace PhotoLibrary.Business.Models
{
    public class LibraryToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}