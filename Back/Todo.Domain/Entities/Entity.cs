using FluentValidation.Results;
using Todo.Domain.Contracts;

namespace Todo.Domain.Entities;

public abstract class Entity : BaseEntity, ITracking
{
    public DateTime CriadoEm { get; set; }
    public int? CriadoPor { get; set; }
    public bool CriadoPorAdmin { get; set; }
    public DateTime AtualizadoEm { get; set; }
    public int? AtualizadoPor { get; set; }
    public bool AtualizadoPorAdmin { get; set; }

    public virtual bool Validar(out ValidationResult validationResult)
    {
        validationResult = new ValidationResult();
        return validationResult.IsValid;
    }
}

public abstract class BaseEntity : IEntity
{
    public int Id { get; set; }
}