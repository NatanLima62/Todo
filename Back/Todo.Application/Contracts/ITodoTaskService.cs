using Todo.Application.Dtos.TodoTask;

namespace Todo.Application.Contracts;

public interface ITodoTaskService
{
    Task<TodoTaskDto?> ObterPorId(int id);
    Task<TodoTaskDto?> Adicionar(AdicionarTodoTaskDto dto);
    Task<TodoTaskDto?> Atualizar(int id, AtualizarTodoTaskDto dto);
    Task Remover(int id);
}