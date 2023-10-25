using FluentValidation.Results;
using Todo.Domain.Contracts;
using Todo.Domain.Validators;

namespace Todo.Domain.Entities;

public class Administrador : Entity, IAggregateRoot
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string? Foto { get; set; }
    
    public override bool Validar(out ValidationResult validationResult)
    {
        validationResult = new AdministradorValidator().Validate(this);
        return validationResult.IsValid;
    }
}