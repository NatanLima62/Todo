using Microsoft.EntityFrameworkCore;
using Todo.Core.Authorization;
using Todo.Domain.Contracts;
using Todo.Infra.Extensions;

namespace Todo.Infra.Contexts;

public sealed class TenantApplicationDbContext : BaseApplicationDbContext
{
    public TenantApplicationDbContext(DbContextOptions<TenantApplicationDbContext> options,
        IAuthenticatedUser authenticatedUser) : base(options, authenticatedUser)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyGlobalFilter<ITenant>(t => t.UsuarioId == AuthenticatedUser.Id);
        base.OnModelCreating(modelBuilder);
    }
}