using System.ComponentModel;

namespace Todo.Core.Enums;

public enum EPathAccess
{
    [Description("assets/public")]
    Public,
    [Description("assets/private")]
    Private
}