using Microsoft.AspNetCore.Http;

namespace Todo.Application.Dtos.Administrador;

public class AdicionarAdministradorDto
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public IFormFile? Foto { get; set; }
}