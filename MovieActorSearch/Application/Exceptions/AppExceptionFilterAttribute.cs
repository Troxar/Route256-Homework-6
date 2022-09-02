using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MovieActorSearch.PostgreDbProvider.Exceptions;

namespace MovieActorSearch.Application.Exceptions;

internal sealed class AppExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ValidationException ex:
                HandleEx(context, ex.Message, HttpStatusCode.BadRequest);
                return;
            
            case ActorNotFoundException ex:
                HandleEx(context, ex.Message, HttpStatusCode.BadRequest);
                return;
            
            case { } ex:
                HandleEx(context, "Boom! " + ex.Message, HttpStatusCode.InternalServerError);
                return;
        }
        
        base.OnException(context);
    }

    private static void HandleEx(ExceptionContext context, string message, HttpStatusCode code)
    {
        context.Result = new JsonResult(new ExceptionReturnObject(message))
        {
            StatusCode = (int)code 
        };
    }
}