using Serilog;
using Serilog.Events;
using Hao.GroupBlog.Web.Extentions;
using Hao.GroupBlog.Manager;
using Hao.GroupBlog.Web.Middlewares;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Server.Kestrel.Core;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console());

    #region Add services to the container.
    builder.Services.AddDbContext(builder.Configuration);
    builder.Services.AddHttpContextAccessor();
    builder.Services.DomainConfigureServices();
    builder.Services.AddControllers();
    builder.Services.AddSwagger();
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddDirectoryBrowser();
    builder.Services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.MaxRequestBodySize = 1024 * 1024 * 256;
    });
    #endregion

    var app = builder.Build();

    #region Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseRouting();
    app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());
    app.Use(async (context, next) =>
    {
        var path = context.Request.Path.Value;
        if (path == "" || path == "/") context.Response.Redirect($"/index.html");
        else await next();
    });
    string spaRoot = builder.Configuration["ClientAppPath"];
    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(spaRoot),
        RequestPath = ""
    });
    app.UseDirectoryBrowser(new DirectoryBrowserOptions()
    {
        FileProvider = new PhysicalFileProvider(spaRoot),
        RequestPath = ""
    });
    app.UseAuthorization();
    app.UseMiddleware<PrivilegeMiddleware>();
    app.MapControllers();
    app.Run();
    #endregion
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

