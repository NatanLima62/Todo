using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using ScottBrady91.AspNetCore.Identity;
using Todo.Application.Contracts;
using Todo.Application.Notifications;
using Todo.Application.Services;
using Todo.Core.Enums;
using Todo.Core.Extensions;
using Todo.Core.Settings;
using Todo.Domain.Entities;
using Todo.Infra;

namespace Todo.Application;

public static class DependencyInjection
{
    public static void SetupSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<UploadSettings>(configuration.GetSection("UploadSettings"));
    }
    public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDbContext(configuration);
        services.AddRepositories();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    public static void AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<INotificator, Notificator>();

        services
            .AddScoped<IPasswordHasher<Administrador>, Argon2PasswordHasher<Administrador>>()
            .AddScoped<IPasswordHasher<Usuario>, Argon2PasswordHasher<Usuario>>();
        
        services
            .AddScoped<IFileService, FileService>()
            .AddScoped<IAdministradorService, AdministradorService>()
            .AddScoped<ITodoListService, TodoListService>()
            .AddScoped<ITodoTaskService, TodoTaskService>()
            .AddScoped<IAdministradorAuthService, AdministradorAuthService>()
            .AddScoped<IUsuarioService, UsuarioService>();
    }
    
    public static void UseStaticFileConfiguration(this IApplicationBuilder app, IConfiguration configuration)
    {
        var uploadSettings = configuration.GetSection("UploadSettings");
        var publicBasePath = uploadSettings.GetValue<string>("PublicBasePath");
        var privateBasePath = uploadSettings.GetValue<string>("PrivateBasePath");

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(publicBasePath),
            RequestPath = $"/{EPathAccess.Public.ToDescriptionString()}"
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(privateBasePath),
            RequestPath = $"/{EPathAccess.Private.ToDescriptionString()}",
            OnPrepareResponse = ctx =>
            {
                if (ctx.Context.User.UsuarioAutenticado()) return;

                // respond HTTP 401 Unauthorized.
                ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                ctx.Context.Response.ContentLength = 0;
                ctx.Context.Response.Body = Stream.Null;
                ctx.Context.Response.Headers.Add("Cache-Control", "no-store");
            }
        });
    }
}