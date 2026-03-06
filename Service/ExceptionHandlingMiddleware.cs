using Microsoft.AspNetCore.Http;
using Serilog;

namespace Assignment.Service
{
    public static class ExceptionHandlingMiddleware
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                try
                {
                    await next(context);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Unhandled exception");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var error = new
                    {
                        message = "An unexpected error occurred",
                        detail = ex.Message // remove in production
                    };

                    await context.Response.WriteAsJsonAsync(error);
                }
            });
        }

    }
}
