using Microsoft.AspNetCore.Builder;

namespace QuizWhois.Domain.Middleware
{
    public static class SwaggerAuthorizedMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerAuthorizedMiddleware>();
        }
    }
}
