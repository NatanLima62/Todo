using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts;
using Todo.Application.Dtos;
using Todo.Application.Dtos.Administrador.Auth;
using Todo.Application.Notifications;

namespace Todo.Api.Controllers.V1.Administracao;

[AllowAnonymous]
[Route("v{version:apiVersion}/administracao/[controller]")]
public class AdministradoresAuthController : BaseController
{
    private readonly IAdministradorAuthService _administradorAuthService;

    public AdministradoresAuthController(INotificator notificator, IAdministradorAuthService administradorAuthService) :
        base(notificator)
    {
        _administradorAuthService = administradorAuthService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Login de administrador", Tags = new[] { "Administração - Autenticação" })]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedObjectResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult?> Login(AdministradorLoginDto administradorLoginDto)
    {
        var token = await _administradorAuthService.Login(administradorLoginDto);
        return token != null ? OkResponse(token) : Unauthorized(new[] { "Usuário e/ou senha incorretos" });
    }
}