namespace Todo.Application.Dtos.TodoTask;

public class AdicionarTodoTaskDto
{
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public DateTime? DeadLine { get; set; }
    public bool Ativo { get; set; }
    public int? TodoId { get; set; }
}