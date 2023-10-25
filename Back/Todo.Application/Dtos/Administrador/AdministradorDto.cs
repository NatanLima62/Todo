namespace Todo.Application.Dtos.Administrador;

public class AdministradorDto
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string? Foto { get; set; }
}