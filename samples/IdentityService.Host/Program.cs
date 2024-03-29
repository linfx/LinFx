using Autofac.Core;
using IdentityService;
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
builder.Logging.ClearProviders().AddSerilog();

// Add services to the container.
builder.Services
    .AddApplication<Application>();

//builder.Services
//    .AddProblemDetails()
//    .AddExceptionHandler<ExceptionHandler>();

builder.Services
    .AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsStaging() || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 中间件
app.UseRequestLocalization(options =>
{
    //var cultures = new[] { "zh-CN", "en-US", "zh-TW" };
    //options.AddSupportedCultures(cultures);
    //options.AddSupportedUICultures(cultures);
    //options.SetDefaultCulture(cultures[0]);

    //// 当Http响应时，将 当前区域信息 设置到 Response Header：Content-Language 中
    //options.ApplyCurrentCultureToResponseHeaders = true;
});

// 异常处理程序中间件
//app.UseExceptionHandler();
//app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), config => config.UseJwtTokenMiddleware(JwtBearerDefaults.AuthenticationScheme));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
