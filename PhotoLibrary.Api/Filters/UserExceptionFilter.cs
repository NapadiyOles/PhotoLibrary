using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoLibrary.Business.Exceptions;

namespace PhotoLibrary.Api.Filters
{
    public class UserExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                ArgumentNullException => new NotFoundObjectResult(context.Exception.Message),
                AuthenticationException => new BadRequestObjectResult(context.Exception.Message),
                ArgumentException => new BadRequestObjectResult(context.Exception.Message),
#if DEBUG
                _ => new BadRequestObjectResult(
                    $"Unhandled error occured. {context.Exception}: {context.Exception.Message}")
#endif
            };
            
            base.OnException(context);
        }
    }
}