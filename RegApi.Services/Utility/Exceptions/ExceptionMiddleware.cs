using Microsoft.AspNetCore.Http;
using RegApi.Services.Extensions.Exceptions;
using RegApi.Services.Models;
using System.Net;

namespace RegApi.Services.Utility.Exceptions
{
    /// <summary>
    /// Middleware for handling exceptions globally in the application.
    /// </summary>
    public class ExceptionMiddleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware to handle exceptions that occur during the request processing.
        /// </summary>
        /// <param name="httpContext">The HTTP context for the current request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var message = ex switch
            {
                AuthenticateException => ex.Message,
                NullReferenceException => ex.Message,
                IdentityException => ex.Message,
                EmailException => "Invalid Email Confirmation Request",
                _ => ex.Message
            };

            await httpContext.Response.WriteAsync(new ErrorDetail
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
