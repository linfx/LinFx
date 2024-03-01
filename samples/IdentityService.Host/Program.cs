using IdentityService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File($"logs/log{DateTime.Now:yyyyMMdd}.txt"))
#if DEBUG
    .WriteTo.Async(c => c.Console())
#endif
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseAutofac();
builder.Logging
    .ClearProviders()
    .AddSerilog();

// Add services to the container.
builder.Services
    .ReplaceConfiguration(builder.Configuration)
    .AddApplication<Application>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), config => config.UseJwtTokenMiddleware(JwtBearerDefaults.AuthenticationScheme));
//app.UseAuthentication();
//app.UseAuthorization();
app.MapControllers();
app.Run();
