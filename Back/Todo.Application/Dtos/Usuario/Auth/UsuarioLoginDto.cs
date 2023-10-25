namespace Todo.Application.Dtos.Usuario.Auth;

public class UsuarioLoginDto
{
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}