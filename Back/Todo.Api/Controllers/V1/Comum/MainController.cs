using Todo.Application.Notifications;
using Todo.Core.Authorization;
using Todo.Core.Enums;

namespace Todo.Api.Controllers.V1.Comum;

[ClaimsAuthorize("TipoUsuario", ETipoUsuario.Comum)]
public abstract class MainController : BaseController
{
    protected MainController(INotificator notificator) : base(notificator)
    {
    }
}