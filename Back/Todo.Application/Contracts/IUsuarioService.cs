using Todo.Application.Dtos.Usuario;

namespace Todo.Application.Contracts;

public interface IUsuarioService
{
    Task<UsuarioDto?> ObterPorId(int id);
    Task<UsuarioDto?> Adicionar(AdicionarUsuarioDto dto);
    Task<UsuarioDto?> Atualizar(int id, AtualizarUsuarioDto dto);
    Task Remover(int id);
}