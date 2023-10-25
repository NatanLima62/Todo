using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.Core.Authorization;
using Todo.Core.Extensions;
using Todo.Domain.Contracts.Repositories;
using Todo.Infra.Contexts;
using Todo.Infra.Repositories;

namespace Todo.Infra;

public static class DependencyInjection
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IAuthenticatedUser>(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

            return httpContextAccessor.UsuarioAutenticado()
                ? new AuthenticatedUser(httpContextAccessor)
                : new AuthenticatedUser();
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            options.UseMySql(connectionString, serverVersion);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
        
        services.AddDbContext<TenantApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            options.UseMySql(connectionString, serverVersion);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });

        services.AddScoped<BaseApplicationDbContext>(serviceProvider =>
        {
            var authenticatedUser = serviceProvider.GetRequiredService<IAuthenticatedUser>();
            if (authenticatedUser is { UsuarioLogado: true, UsuarioComum: true })
            {
                return serviceProvider.GetRequiredService<TenantApplicationDbContext>();
            }

            return serviceProvider.GetRequiredService<ApplicationDbContext>();
        });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IAdministradorRepository, AdministradorRepository>()
            .AddScoped<ITodoListRepository, TodoListRepository>()
            .AddScoped<ITodoTaskRepository, TodoTaskRepository>()
            .AddScoped<IUsuarioRepository, UsuarioRepository>();
    }
    
    public static void UseMigrations(this IApplicationBuilder app, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }
}