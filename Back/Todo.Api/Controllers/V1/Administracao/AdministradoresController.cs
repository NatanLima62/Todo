using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts;
using Todo.Application.Dtos.Administrador;
using Todo.Application.Notifications;

namespace Todo.Api.Controllers.V1.Administracao;

[Route("v{version:apiVersion}/administracao/[controller]")]
public class AdministradoresController : MainController
{
    private readonly IAdministradorService _administradorService;

    public AdministradoresController(INotificator notificator, IAdministradorService administradorService) :
        base(notificator)
    {
        _administradorService = administradorService;
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Obter um administrador", Tags = new[] { "Administração - Administrador" })]
    [ProducesResponseType(typeof(AdministradorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var administrador = await _administradorService.ObterPorId(id);
        return OkResponse(administrador);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Criar um administrador", Tags = new[] { "Administração - Administrador" })]
    [ProducesResponseType(typeof(AdministradorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Adicionar([FromForm] AdicionarAdministradorDto dto)
    {
        var administrador = await _administradorService.Adicionar(dto);
        return OkResponse(administrador);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Atualizar um administrador", Tags = new[] { "Administração - Administrador" })]
    [ProducesResponseType(typeof(AdministradorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromForm] AtualizarAdministradorDto dto)
    {
        var administrador = await _administradorService.Atualizar(id, dto);
        return OkResponse(administrador);
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Remover um administrador", Tags = new[] { "Administração - Administrador" })]
    [ProducesResponseType(typeof(AdministradorDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Remover(int id)
    {
        await _administradorService.Remover(id);
        return NoContentResponse();
    }
}