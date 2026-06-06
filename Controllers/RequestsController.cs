using HrPortal.Api.DTOs;
using HrPortal.Api.Infrastructure;
using HrPortal.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrPortal.Api.Controllers;

[ApiController]
[Route("api/requests")]
public sealed class RequestsController : ControllerBase
{
    [HttpGet("my")]
    public async Task<ActionResult<IReadOnlyList<RequestItemDto>>> GetMyRequests(
        [FromServices] IRequestService requestService,
        [FromServices] ICurrentEmployeeContext employeeContext,
        CancellationToken cancellationToken)
    {
        var result = await requestService.GetMyRequestsAsync(employeeContext.GetEmployeeId(), cancellationToken);
        return Ok(result);
    }
}
