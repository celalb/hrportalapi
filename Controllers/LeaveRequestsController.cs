using HrPortal.Api.DTOs;
using HrPortal.Api.Infrastructure;
using HrPortal.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrPortal.Api.Controllers;

[ApiController]
[Route("api/leave-requests")]
public sealed class LeaveRequestsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateRequestResponseDto>> Create(
        [FromBody] CreateLeaveRequestDto request,
        [FromServices] IRequestService requestService,
        [FromServices] ICurrentEmployeeContext employeeContext,
        CancellationToken cancellationToken)
    {
        var result = await requestService.CreateLeaveAsync(employeeContext.GetEmployeeId(), request, cancellationToken);
        return Created($"/api/requests/my", result);
    }
}
