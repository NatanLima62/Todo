using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts;
using Todo.Domain.Contracts.Paginacao;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;
using Todo.Infra.Contexts;
using Todo.Infra.Extensions;

namespace Todo.Infra.Repositories;

public abstract class Repository<T> : IRepository<T> where T : BaseEntity, IAggregateRoot, new()
{
    private bool _isDisposed;
    private readonly DbSet<T> _dbSet;
    protected readonly BaseApplicationDbContext Context;

    public IUnitOfWork UnitOfWork => Context;

    protected Repository(BaseApplicationDbContext context)
    {
        Context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IResultadoPaginado<T>> Buscar(IBuscaPaginada<T> filtro)
    {
        var queryable = _dbSet.AsQueryable();
        
        filtro.AplicarFiltro(ref queryable);
        filtro.AplicarOrdenacao(ref queryable);
        
        return await queryable.BuscarPaginadoAsync(filtro.Pagina, filtro.TamanhoPagina);
    }
    
    public async Task<IResultadoPaginado<T>> Buscar(IQueryable<T> queryable, IBuscaPaginada<T> filtro)
    {
        filtro.AplicarFiltro(ref queryable);
        filtro.AplicarOrdenacao(ref queryable);
        
        return await queryable.BuscarPaginadoAsync(filtro.Pagina, filtro.TamanhoPagina);
    }

    public async Task<T?> FistOrDefault(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AsNoTrackingWithIdentityResolution().Where(expression).FirstOrDefaultAsync();
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            Context.Dispose();
        }

        _isDisposed = true;
    }

    ~Repository()
    {
        Dispose(false);
    }
}