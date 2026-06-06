using System.Security.Claims;

namespace HrPortal.Api.Infrastructure;

public sealed class CurrentEmployeeContext : ICurrentEmployeeContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentEmployeeContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetEmployeeId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            throw new ApiException(StatusCodes.Status401Unauthorized, "Employee context is unavailable.");
        }

        var fromClaim = httpContext.User.FindFirstValue("employee_id")
                        ?? httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrWhiteSpace(fromClaim))
        {
            return fromClaim;
        }

        if (httpContext.Request.Headers.TryGetValue("X-Employee-Id", out var values) && !string.IsNullOrWhiteSpace(values))
        {
            return values.ToString();
        }

        throw new ApiException(StatusCodes.Status400BadRequest, "Employee id is required. Provide claim or X-Employee-Id header.");
    }
}
