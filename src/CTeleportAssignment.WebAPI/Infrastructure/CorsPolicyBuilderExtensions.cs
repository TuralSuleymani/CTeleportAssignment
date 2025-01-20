using Microsoft.AspNetCore.Cors.Infrastructure;

namespace CTeleportAssignment.WebAPI.Infrastructure
{
    public static class CorsPolicyBuilderExtensions
    {
        public static CorsPolicyBuilder AllowAny(this CorsPolicyBuilder corsPolicyBuilder)
        {
            return corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }
    }
}
