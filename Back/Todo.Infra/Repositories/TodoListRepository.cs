using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;
using Todo.Infra.Contexts;

namespace Todo.Infra.Repositories;

public class TodoListRepository : Repository<TodoList>, ITodoListRepository
{
    public TodoListRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public async Task<TodoList?> ObterPorId(int id)
    {
        return await Context.Todos.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(c => c.Id == id);
    }

    public void Criar(TodoList usuario)
    {
        Context.Todos.Add(usuario);
    }

    public void Alterar(TodoList usuario)
    {
        Context.Todos.Update(usuario);
    }

    public void Remover(TodoList usuario)
    {
        Context.Todos.Remove(usuario);
    }
}