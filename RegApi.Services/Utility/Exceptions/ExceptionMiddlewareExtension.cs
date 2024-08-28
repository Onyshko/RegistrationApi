using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using RegApi.Services.Models;
using System.Net;

namespace RegApi.Services.Utility.Exceptions
{
    /// <summary>
    /// Extension methods for configuring exception handling middleware in the application.
    /// </summary>
    public static class ExceptionMiddlewareExtension
    {
        /// <summary>
        /// Configures the built-in exception handler middleware to handle exceptions globally.
        /// </summary>
        /// <param name="app">The web application builder instance.</param>
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetail
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error"
                        }.ToString());
                    }
                });
            });
        }

        /// <summary>
        /// Configures custom exception middleware for handling exceptions with custom logic.
        /// </summary>
        /// <param name="app">The web application builder instance.</param>
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
