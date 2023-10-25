using Todo.Application.Dtos.Administrador;

namespace Todo.Application.Contracts;

public interface IAdministradorService
{
    Task<AdministradorDto?> ObterPorId(int id);
    Task<AdministradorDto?> Adicionar(AdicionarAdministradorDto dto);
    Task<AdministradorDto?> Atualizar(int id, AtualizarAdministradorDto dto);
    Task Remover(int id);
}