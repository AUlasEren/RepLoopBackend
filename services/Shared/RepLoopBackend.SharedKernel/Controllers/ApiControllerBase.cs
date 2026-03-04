using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RepLoopBackend.SharedKernel.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected Guid CurrentUserId =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : throw new UnauthorizedAccessException("Kullanıcı kimliği doğrulanamadı.");
}
