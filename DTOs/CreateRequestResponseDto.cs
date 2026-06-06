using HrPortal.Api.Models;

namespace HrPortal.Api.DTOs;

public sealed class CreateRequestResponseDto
{
    public Guid Id { get; init; }
    public RequestStatus Status { get; init; }
}
