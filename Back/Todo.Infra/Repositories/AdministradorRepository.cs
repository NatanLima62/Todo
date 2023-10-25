using Microsoft.EntityFrameworkCore;
using Todo.Domain.Contracts.Paginacao;
using Todo.Domain.Contracts.Repositories;
using Todo.Domain.Entities;
using Todo.Infra.Contexts;

namespace Todo.Infra.Repositories;

public class AdministradorRepository : Repository<Administrador>, IAdministradorRepository
{
    public AdministradorRepository(BaseApplicationDbContext context) : base(context)
    {
    }

    public async Task<Administrador?> ObterPorId(int id)
    {
        return await Context.Administradores.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Administrador?> ObterPorEmail(string email)
    {
        return await Context.Administradores.FirstOrDefaultAsync(c => c.Email == email);
    }

    public void Criar(Administrador usuario)
    {
        Context.Administradores.Add(usuario);
    }

    public void Alterar(Administrador usuario)
    {
        Context.Administradores.Update(usuario);
    }

    public void Remover(Administrador usuario)
    {
        Context.Administradores.Remove(usuario);
    }

    public override async Task<IResultadoPaginado<Administrador>> Buscar(IBuscaPaginada<Administrador> filtro)
    {
        var queryable = Context.Administradores.AsQueryable();
        return await Buscar(queryable, filtro);
    }
}