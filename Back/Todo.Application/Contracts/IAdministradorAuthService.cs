using Todo.Application.Dtos;
using Todo.Application.Dtos.Administrador.Auth;

namespace Todo.Application.Contracts;

public interface IAdministradorAuthService
{
    Task<TokenDto?> Login(AdministradorLoginDto administradorLogin);
}