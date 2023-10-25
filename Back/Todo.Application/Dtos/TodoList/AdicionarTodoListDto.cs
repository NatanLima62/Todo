namespace Todo.Application.Dtos.TodoList;

public class AdicionarTodoListDto
{
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public DateTime? DeadLine { get; set; }
    public bool Ativo { get; set; }
}