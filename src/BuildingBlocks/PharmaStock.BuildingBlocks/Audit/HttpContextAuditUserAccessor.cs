using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PharmaStock.BuildingBlocks.Audit;

public sealed class HttpContextAuditUserAccessor(IHttpContextAccessor httpContextAccessor) : IAuditUserAccessor
{
    public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
