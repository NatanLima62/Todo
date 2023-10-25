namespace Todo.Domain.Contracts;

public interface IUnitOfWork
{
    Task<bool> Commit();
}