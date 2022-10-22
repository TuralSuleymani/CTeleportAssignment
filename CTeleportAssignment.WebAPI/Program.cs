using CTeleportAssignment.WebAPI.Infrastructure;
using Serilog;
using CTeleportAssignment.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCaching()
                .AddCircuitBreaker();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add all services related to API
builder.Services.AddApiServices();

//configuring CORS for all. TODO : it is for testing purpose. Don't deploy with AllowAny()!
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyTypes.AllowAll, policy =>
    {
        policy.AllowAny();
    });
});

//configuring serilog for reading config from the configuration
builder.Host.UseSerilog((ctx,lc) => lc.ReadFrom.Configuration(ctx.Configuration));

//read settings for api configuration
builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddMediatr();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandling();

app.UseCors(CorsPolicyTypes.AllowAll);

app.UseSerilogRequestLogging();

app.UseResponseCaching();

app.UseCacheControl();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
