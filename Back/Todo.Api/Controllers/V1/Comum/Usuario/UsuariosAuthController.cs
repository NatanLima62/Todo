using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts;
using Todo.Application.Dtos;
using Todo.Application.Dtos.Usuario.Auth;
using Todo.Application.Notifications;

namespace Todo.Api.Controllers.V1.Comum.Usuario;

[AllowAnonymous]
[Route("v{version:apiVersion}/comum/[controller]")]
public class UsuariosAuthController : BaseController
{
    private readonly IUsuarioAuthService _usuarioAuthService;

    public UsuariosAuthController(INotificator notificator, IUsuarioAuthService administradorAuthService) :
        base(notificator)
    {
        _usuarioAuthService = administradorAuthService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Login de usuários", Tags = new[] { "Comum - Usuario Autenticação" })]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult?> Login(UsuarioLoginDto usuarioLoginDto)
    {
        var token = await _usuarioAuthService.Login(usuarioLoginDto);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Usuário e/ou senha incorretos" });
    }
}