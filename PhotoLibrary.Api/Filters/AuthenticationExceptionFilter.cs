using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhotoLibrary.Business.Exceptions;

namespace PhotoLibrary.Api.Filters
{
    /// <summary>
    /// Filters exceptions from Authentication controller
    /// </summary>
    public class AuthenticationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                AuthenticationException => new BadRequestObjectResult(context.Exception.Message),
                UnregisteredException => new BadRequestObjectResult(context.Exception.Message),
#if DEBUG
                _ => new BadRequestObjectResult(
                    $"Unhandled error occured. {context.Exception}: {context.Exception.Message}")
#endif
            };
            
            base.OnException(context);
        }
    }
}