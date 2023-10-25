using FluentValidation;
using Todo.Domain.Entities;

namespace Todo.Domain.Validators;

public class TodoTaskValidator : AbstractValidator<TodoTask>
{
    public TodoTaskValidator()
    {
        RuleFor(c => c.Nome)
            .Length(3, 80)
            .WithMessage("Nome deve ter no mínimo 3 e no máximo 80 caracteres");
        
        RuleFor(c => c.Descricao)
            .Length(3, 150)
            .WithMessage("Descricao deve ter no mínimo 3 e no máximo 80 caracteres");
    }
}