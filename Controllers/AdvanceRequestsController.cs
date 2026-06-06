using HrPortal.Api.DTOs;
using HrPortal.Api.Infrastructure;
using HrPortal.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrPortal.Api.Controllers;

[ApiController]
[Route("api/advance-requests")]
public sealed class AdvanceRequestsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateRequestResponseDto>> Create(
        [FromBody] CreateAdvanceRequestDto request,
        [FromServices] IRequestService requestService,
        [FromServices] ICurrentEmployeeContext employeeContext,
        CancellationToken cancellationToken)
    {
        var result = await requestService.CreateAdvanceAsync(employeeContext.GetEmployeeId(), request, cancellationToken);
        return Created($"/api/requests/my", result);
    }
}
