using Todo.Domain.Contracts.Paginacao;
using Todo.Domain.Entities;

namespace Todo.Domain.Contracts.Repositories;

public interface ITodoTaskRepository : IRepository<TodoTask>
{
    Task<TodoTask?> ObterPorId(int id);
    Task<IResultadoPaginado<TodoTask>> Buscar(IBuscaPaginada<TodoTask> filtro);
    void Criar(TodoTask usuario);
    void Alterar(TodoTask usuario);
    void Remover(TodoTask usuario);
}