using Authentication.Application.CustomExceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.Filter
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            switch (exception)
            {
                case AuthenticationNotFoundException e:
                    httpContext.Response.StatusCode = 404;
                    problemDetails.Title = "Not Found";
                    problemDetails.Detail = e.Message;
                    break;

                case AuthenticationBadRequestException e:
                    httpContext.Response.StatusCode = 400;
                    problemDetails.Title = "Bad Request";
                    problemDetails.Detail = e.Message;
                    break;

                case AuthenticationUnauthorizedException e:
                    httpContext.Response.StatusCode = 401;
                    problemDetails.Title = "Unauthorized";
                    problemDetails.Detail = e.Message;
                    break;

                default:
                    httpContext.Response.StatusCode = 500;
                    problemDetails.Title = "Internal Server Error";
                    problemDetails.Detail = "Errore imprevisto";
                    break;
            }
            logger.LogError("{ProblemDetailsTitle}: {Detail}", problemDetails.Title, problemDetails.Detail);
            problemDetails.Status = httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
