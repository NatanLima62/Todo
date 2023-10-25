using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;
using Todo.Infra.Contexts;

namespace Todo.Infra.Repositories;

public class TodoTaskRepository : Repository<TodoTask>, ITodoTaksRepository
{
    public TodoTaskRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public async Task<TodoTask?> ObterPorId(int id)
    {
        return await Context.Tasks.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(c => c.Id == id);
    }

    public void Criar(TodoTask usuario)
    {
        Context.Tasks.Add(usuario);
    }

    public void Alterar(TodoTask usuario)
    {
        Context.Tasks.Update(usuario);
    }

    public void Remover(TodoTask usuario)
    {
        Context.Tasks.Remove(usuario);
    }
}