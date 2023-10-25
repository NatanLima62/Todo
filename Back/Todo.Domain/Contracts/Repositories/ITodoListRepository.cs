using Todo.Domain.Contracts.Paginacao;
using Todo.Domain.Entities;

namespace Todo.Domain.Contracts.Repositories;

public interface ITodoListRepository : IRepository<TodoList>
{
    Task<TodoList?> ObterPorId(int id);
    Task<IResultadoPaginado<TodoList>> Buscar(IBuscaPaginada<TodoList> filtro);
    void Criar(TodoList usuario);
    void Alterar(TodoList usuario);
    void Remover(TodoList usuario);
}