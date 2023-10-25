using Todo.Domain.Contracts.Paginacao;
using Todo.Domain.Entities;

namespace Todo.Domain.Contracts.Repositories;

public interface IAdministradorRepository : IRepository<Administrador>
{
    Task<Administrador?> ObterPorId(int id);
    Task<Administrador?> ObterPorEmail(string email);

    Task<IResultadoPaginado<Administrador>> Buscar(IBuscaPaginada<Administrador> filtro);
    void Criar(Administrador administrador);
    void Alterar(Administrador administrador);
    void Remover(Administrador administrador);
}