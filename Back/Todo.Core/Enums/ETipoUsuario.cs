using System.ComponentModel;

namespace Todo.Core.Enums;

public enum ETipoUsuario
{
    [Description("Administrador")] Administrador = 1,
    [Description("Comum")] Comum = 2
}