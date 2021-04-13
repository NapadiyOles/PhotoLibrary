using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PhotoLibrary.Business.Exceptions;

namespace PhotoLibrary.Api.Filters
{
    /// <summary>
    /// Filters exceptions from Picture controller
    /// </summary>
    public class PictureExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                ArgumentOutOfRangeException => new BadRequestObjectResult(context.Exception.Message),
                ArgumentNullException => new BadRequestObjectResult(context.Exception.Message),
                ArgumentException => new BadRequestObjectResult(context.Exception.Message),
                IdentityException => new BadRequestObjectResult(context.Exception.Message),
                NullReferenceException => new NotFoundObjectResult(context.Exception.Message),
                DbUpdateConcurrencyException => new NotFoundObjectResult(context.Exception.Message),
#if DEBUG
                _ => new BadRequestObjectResult(
                    $"Unhandled error occured. {context.Exception}: {context.Exception.Message}")
#endif
            };
            
            base.OnException(context);
        }
    }
}