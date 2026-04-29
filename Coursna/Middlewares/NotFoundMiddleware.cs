using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Coursna.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = "Endpoint not found",
                    statusCode = 404
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class NotFoundMiddlewareExtensions
    {
        public static IApplicationBuilder UseNotFoundMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NotFoundMiddleware>();
        }
    }
}
