using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoLibrary.Business.Exceptions;

namespace PhotoLibrary.Api.Filters
{
    public class AuthenticationExceptionFilter : ExceptionFilterAttribute
    {
        private object _error;
        
        public override void OnException(ExceptionContext context)
        {
            _error = context.Exception switch
            {
                AuthenticationException => context.Exception.Message,
                UnregisteredException => context.Exception.Message,
                _ => "Unhandled error occured. " + context.Exception.Message
            };
            
            context.Result = new BadRequestObjectResult(_error);
            base.OnException(context);
        }
    }
}