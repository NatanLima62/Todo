using FluentValidation.Results;
using Todo.Domain.Contracts;
using Todo.Domain.Validators;

namespace Todo.Domain.Entities;

public class TodoTask : Entity, IAggregateRoot, ITenant
{
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public DateTime? DeadLine { get; set; }
    public bool Ativo { get; set; }
    public int UsuarioId { get; set; }
    public int? TodoId { get; set; }
    
    public Usuario Usuario { get; set; } = null!;
    public TodoList Todo { get; set; } = null!;
    
    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new TodoTaskValidator().Validate(this);
        return validationResult.IsValid;
    }
}