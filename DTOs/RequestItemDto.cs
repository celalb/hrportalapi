using HrPortal.Api.Models;

namespace HrPortal.Api.DTOs;

public sealed class RequestItemDto
{
    public Guid Id { get; init; }
    public string EmployeeId { get; init; } = string.Empty;
    public RequestType Type { get; init; }
    public string Title { get; init; } = string.Empty;
    public RequestStatus Status { get; init; }
    public string? AdminNote { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? ReviewedAtUtc { get; init; }
    public object? Payload { get; init; }
}
