using System.Reflection;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Authorization;
using Todo.Domain.Contracts;
using Todo.Domain.Entities;
using Todo.Infra.Converters;
using Todo.Infra.Extensions;

namespace Todo.Infra.Contexts;

public abstract class BaseApplicationDbContext : DbContext, IUnitOfWork
{
    protected readonly IAuthenticatedUser AuthenticatedUser;

    protected BaseApplicationDbContext(DbContextOptions options, IAuthenticatedUser authenticatedUser) : base(options)
    {
        AuthenticatedUser = authenticatedUser;
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Administrador> Administradores { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssemblyWithServiceInjection(Assembly.GetExecutingAssembly())
            .HasCharSet("utf8mb4")
            .UseCollation("utf8mb4_0900_ai_ci")
            .UseGuidCollation(string.Empty);

        ApplyConfigurations(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTrackingChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> Commit() => await SaveChangesAsync() > 0;

    private static void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.ApplyEntityConfiguration();
        modelBuilder.ApplyTrackingConfiguration();
    }

    private void ApplyTrackingChanges()
    {
        var entries = ChangeTracker
            .Entries().Where(e => e.Entity is ITracking && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            ((ITracking)entityEntry.Entity).AtualizadoEm = DateTime.Now;
            ((ITracking)entityEntry.Entity).AtualizadoPor = AuthenticatedUser.Id;
            ((ITracking)entityEntry.Entity).AtualizadoPorAdmin = AuthenticatedUser.UsuarioAdministrador;

            if (entityEntry.State != EntityState.Added)
                continue;

            ((ITracking)entityEntry.Entity).CriadoEm = DateTime.Now;
            ((ITracking)entityEntry.Entity).CriadoPor = AuthenticatedUser.Id;
            ((ITracking)entityEntry.Entity).CriadoPorAdmin = AuthenticatedUser.UsuarioAdministrador;
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateOnly>()
            .HaveConversion<DateOnlyCustomConverter>()
            .HaveColumnType("DATE");

        configurationBuilder
            .Properties<TimeOnly>()
            .HaveConversion<TimeOnlyCustomConverter>()
            .HaveColumnType("TIME");
    }
}