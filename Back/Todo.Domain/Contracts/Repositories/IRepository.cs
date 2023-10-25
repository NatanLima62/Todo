using System.Linq.Expressions;
using Todo.Domain.Contracts.Paginacao;
using Todo.Domain.Entities;

namespace Todo.Domain.Contracts.Repositories;

public interface IRepository<T> : IDisposable where T : BaseEntity, IAggregateRoot
{
    public IUnitOfWork UnitOfWork { get; }
    Task<IResultadoPaginado<T>> Buscar(IBuscaPaginada<T> filtro);
    Task<T?> FistOrDefault(Expression<Func<T, bool>> expression);
}