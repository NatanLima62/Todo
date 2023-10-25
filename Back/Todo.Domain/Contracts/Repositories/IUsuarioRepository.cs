using Todo.Domain.Contracts.Paginacao;
using Todo.Domain.Entities;

namespace Todo.Domain.Contracts.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> ObterPorId(int id);
    Task<Usuario?> ObterPorEmail(string email);

    Task<IResultadoPaginado<Usuario>> Buscar(IBuscaPaginada<Usuario> filtro);
    void Criar(Usuario usuario);
    void Alterar(Usuario usuario);
    void Remover(Usuario usuario);
}