using Todo.Application.Dtos;
using Todo.Application.Dtos.Usuario.Auth;

namespace Todo.Application.Contracts;

public interface IUsuarioAuthService
{
    Task<TokenDto?> Login(UsuarioLoginDto usuarioLoginDto);
}