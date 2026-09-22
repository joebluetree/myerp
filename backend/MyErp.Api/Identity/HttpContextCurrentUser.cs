using System.Security.Claims;
using MyErp.Common.Interfaces;

namespace MyErp.Api.Identity;

/// <summary>
/// Exposes the authenticated principal of the current request to the modules,
/// which only ever see the <see cref="ICurrentUser"/> abstraction.
/// </summary>
public sealed class HttpContextCurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName => Principal?.Identity?.Name;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;
}
