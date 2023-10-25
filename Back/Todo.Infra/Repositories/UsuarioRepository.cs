using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Paginacao;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;
using Todo.Infra.Contexts;

namespace Todo.Infra.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> ObterPorId(int id)
    {
        return await Context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Usuario?> ObterPorEmail(string email)
    {
        return await Context.Usuarios.FirstOrDefaultAsync(c => c.Email == email);
    }

    public void Criar(Usuario usuario)
    {
        Context.Usuarios.Add(usuario);
    }

    public void Alterar(Usuario usuario)
    {
        Context.Usuarios.Update(usuario);
    }

    public void Remover(Usuario usuario)
    {
        Context.Usuarios.Remove(usuario);
    }

    public override async Task<IResultadoPaginado<Usuario>> Buscar(IBuscaPaginada<Usuario> filtro)
    {
        var queryable = Context.Usuarios.AsQueryable();
        return await Buscar(queryable, filtro);
    }
}