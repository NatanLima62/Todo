using Microsoft.AspNetCore.Http;
using Todo.Core.Enums;
using Todo.Core.Extensions;

namespace Todo.Core.Authorization;

public interface IAuthenticatedUser
{
    public int Id { get; }
    public ETipoUsuario? TipoUsuario { get; }
    public string Nome { get; }
    public string Email { get; }
    public bool UsuarioLogado { get; }
    public bool UsuarioComum { get; }
    public bool UsuarioAdministrador { get; }
}

public class AuthenticatedUser : IAuthenticatedUser
{
    public AuthenticatedUser()
    {
    }

    public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
    {
        Id = httpContextAccessor.ObterIdUsuario()!.Value;
        TipoUsuario = httpContextAccessor.ObterTipoUsuario()!.Value;
        Nome = httpContextAccessor.ObterNomeUsuario()!;
        Email = httpContextAccessor.ObterEmailUsuario()!;
    }

    public int Id { get; } = -1;
    public ETipoUsuario? TipoUsuario { get; }
    public string Nome { get; } = string.Empty;
    public string Email { get; } = string.Empty;
    public bool UsuarioLogado => Id > 0;
    public bool UsuarioComum => TipoUsuario is ETipoUsuario.Administrador;
    public bool UsuarioAdministrador => TipoUsuario is ETipoUsuario.Administrador;
}