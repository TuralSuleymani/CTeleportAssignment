using CTeleportAssignment.WebAPI.Middleware;

namespace CTeleportAssignment.WebAPI.Extensions
{
    public static class WebApplicationExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this WebApplication app)
        {
           return app.UseMiddleware<ExceptionMiddleware>();
        }

        public static IApplicationBuilder UseCacheControl(this WebApplication app)
        {
            return app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders()
                .CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                {
                    Public = true,
                    MaxAge = TimeSpan.FromMinutes(10)
                };
                await next();
            });
        }
    }
}
