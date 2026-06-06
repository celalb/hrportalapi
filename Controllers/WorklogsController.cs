using HrPortal.Api.DTOs;
using HrPortal.Api.Infrastructure;
using HrPortal.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrPortal.Api.Controllers;

[ApiController]
[Route("api/worklogs")]
public sealed class WorklogsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateRequestResponseDto>> Create(
        [FromBody] CreateWorklogDto request,
        [FromServices] IRequestService requestService,
        [FromServices] ICurrentEmployeeContext employeeContext,
        CancellationToken cancellationToken)
    {
        var result = await requestService.CreateWorklogAsync(employeeContext.GetEmployeeId(), request, cancellationToken);
        return Created($"/api/requests/my", result);
    }
}
