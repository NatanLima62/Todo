using Todo.Application.Dtos.TodoList;

namespace Todo.Application.Contracts;

public interface ITodoListService
{
    Task<TodoListDto?> ObterPorId(int id);
    Task<TodoListDto?> Adicionar(AdicionarTodoListDto dto);
    Task<TodoListDto?> Atualizar(int id, AtualizarTodoListDto dto);
    Task Remover(int id);
}