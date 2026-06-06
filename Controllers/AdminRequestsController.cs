using HrPortal.Api.DTOs;
using HrPortal.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HrPortal.Api.Controllers;

[ApiController]
[Route("api/admin/requests")]
public sealed class AdminRequestsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RequestItemDto>>> GetAll(
        [FromServices] IRequestService requestService,
        CancellationToken cancellationToken)
    {
        var result = await requestService.GetAdminRequestsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] Guid id,
        [FromBody] UpdateRequestStatusDto request,
        [FromServices] IRequestService requestService,
        CancellationToken cancellationToken)
    {
        await requestService.UpdateStatusAsync(id, request, cancellationToken);
        return NoContent();
    }
}
