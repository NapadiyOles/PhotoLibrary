using System;
using System.Collections.Generic;

namespace PhotoLibrary.Business.Exceptions
{
    public class AuthenticationException : AggregateException
    {
        public AuthenticationException()
        {
        }

        public AuthenticationException(string message) : base(message)
        {
        }

        public AuthenticationException(IEnumerable<Exception> innerExceptions) : base(innerExceptions)
        {
        }
    }
}