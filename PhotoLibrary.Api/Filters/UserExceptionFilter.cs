using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoLibrary.Business.Exceptions;

namespace PhotoLibrary.Api.Filters
{
    public class UserExceptionFilter : ExceptionFilterAttribute
    {
        private object _error;

        public override void OnException(ExceptionContext context)
        {
            _error = context.Exception switch
            {
                ArgumentNullException => context.Exception.Message,
                AuthenticationException => context.Exception.Message,
                ArgumentException => context.Exception.Message,
                _ => "Unhandled error occured. " + context.Exception.Message
            };
            
            context.Result = new BadRequestObjectResult(_error);
            base.OnException(context);
        }
    }
}