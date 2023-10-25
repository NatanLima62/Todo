using Microsoft.EntityFrameworkCore;
using Todo.Core.Authorization;

namespace Todo.Infra.Contexts;

public sealed class ApplicationDbContext : BaseApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAuthenticatedUser authenticatedUser) :
        base(options, authenticatedUser)
    {
    }
}