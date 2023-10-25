using Todo.Domain.Contracts;

namespace Todo.Domain.Entities;

public class TodoList : Entity, IAggregateRoot, ITenant
{
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public DateTime? DeadLine { get; set; }
    public bool Ativo { get; set; }
    public int UsuarioId { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public List<TodoTask> Tasks { get; set; } = new();
}