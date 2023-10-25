using Microsoft.AspNetCore.Http;

namespace Todo.Application.Dtos.Usuario;

public class AtualizarUsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IFormFile? Foto { get; set; }
}