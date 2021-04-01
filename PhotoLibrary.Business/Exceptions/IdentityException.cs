using System;

namespace PhotoLibrary.Business.Exceptions
{
    public class IdentityException : Exception
    {
        public IdentityException(string message) : base(message)
        {
        }
    }
}