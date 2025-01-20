using CTeleportAssignment.WebAPI.Infrastructure;
using Serilog;
using CTeleportAssignment.WebAPI.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline
        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        // Add API-related services
        services.AddResponseCaching()
                .AddCircuitBreaker();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Add API-specific services
        services.AddApiServices();

        // Configure CORS
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyTypes.AllowAll, policy =>
            {
                policy.AllowAny(); // For testing purposes only
            });
        });

        // Configure Serilog
        builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

        // Add API configuration
        services.AddApiConfiguration(builder.Configuration);
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        // Use Swagger in development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Add middleware to the pipeline
        app.UseCors(CorsPolicyTypes.AllowAll);
        app.UseSerilogRequestLogging();
        app.UseResponseCaching();
        app.UseCacheControl();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        // Map controllers
        app.MapControllers();
    }
}
