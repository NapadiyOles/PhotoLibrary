using System;

namespace PhotoLibrary.Business.Exceptions
{
    public class UnregisteredException : Exception
    {
        public UnregisteredException()
        {
        }

        public UnregisteredException(string message) : base(message)
        {
        }
    }
}