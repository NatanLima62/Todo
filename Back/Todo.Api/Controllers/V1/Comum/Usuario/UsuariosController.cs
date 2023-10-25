using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Todo.Application.Contracts;
using Todo.Application.Dtos.Usuario;
using Todo.Application.Notifications;

namespace Todo.Api.Controllers.V1.Comum.Usuario;

[Route("v{version:apiVersion}/usuario/[controller]")]
public class UsuariosController : MainController
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(INotificator notificator, IUsuarioService usuarioService) : base(notificator)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Obter um usuario", Tags = new[] { "Comum - Usuario" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var usuario = await _usuarioService.ObterPorId(id);
        return OkResponse(usuario);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Criar um usuario", Tags = new[] { "Comum - Usuario" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Adicionar([FromForm] AdicionarUsuarioDto dto)
    {
        var usuario = await _usuarioService.Adicionar(dto);
        return OkResponse(usuario);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Atualizar um usuario", Tags = new[] { "Comum - Usuario" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Atualizar(int id, [FromForm] AtualizarUsuarioDto dto)
    {
        var usuario = await _usuarioService.Atualizar(id, dto);
        return OkResponse(usuario);
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Remover um usuario", Tags = new[] { "Comum - Usuario" })]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Remover(int id)
    {
        await _usuarioService.Remover(id);
        return NoContentResponse();
    }
}