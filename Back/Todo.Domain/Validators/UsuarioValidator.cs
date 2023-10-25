using FluentValidation;
using Todo.Domain.Entities;

namespace Todo.Domain.Validators;

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(c => c.Nome)
            .Length(3, 80)
            .WithMessage("Nome deve ter no mínimo 3 e no máximo 80 caracteres");
        RuleFor(c => c.Email)
            .EmailAddress();
        RuleFor(c => c.Senha)
            .Length(3, 80)
            .WithMessage("Senha deve ter no mínimo 8 e no máximo 16 caracteres");
    }
}