using Microsoft.AspNetCore.Http;
using Todo.Core.Enums;

namespace Todo.Core.Extensions;

public static class HttpContextAccessorExtension
{
    public static bool UsuarioAutenticado(this IHttpContextAccessor? contextAccessor)
    {
        return contextAccessor?.HttpContext?.User.UsuarioAutenticado() ?? false;
    }

    public static ETipoUsuario? ObterTipoUsuario(this IHttpContextAccessor? contextAccessor)
    {
        var tipo = contextAccessor?.HttpContext?.User.ObterTipoUsuario() ?? string.Empty;
        return string.IsNullOrWhiteSpace(tipo) ? null : Enum.Parse<ETipoUsuario>(tipo);
    }

    public static int? ObterIdUsuario(this IHttpContextAccessor? contextAccessor)
    {
        var id = contextAccessor?.HttpContext?.User.ObterIdUsuario() ?? string.Empty;
        return string.IsNullOrWhiteSpace(id) ? null : int.Parse(id);
    }

    public static string? ObterNomeUsuario(this IHttpContextAccessor? contextAccessor)
    {
        var nome = contextAccessor?.HttpContext?.User.ObterNomeUsuario() ?? string.Empty;
        return string.IsNullOrWhiteSpace(nome) ? null : nome;
    }

    public static string? ObterEmailUsuario(this IHttpContextAccessor? contextAccessor)
    {
        var email = contextAccessor?.HttpContext?.User.ObterEmailUsuario() ?? string.Empty;
        return string.IsNullOrWhiteSpace(email) ? null : email;
    }
}