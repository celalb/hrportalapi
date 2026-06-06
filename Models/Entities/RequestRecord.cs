namespace HrPortal.Api.Models.Entities;

public sealed class RequestRecord
{
    public Guid Id { get; init; }
    public string EmployeeId { get; init; } = string.Empty;
    public Models.RequestType Type { get; init; }
    public string Title { get; init; } = string.Empty;
    public string PayloadJson { get; init; } = string.Empty;
    public Models.RequestStatus Status { get; set; }
    public string? AdminNote { get; set; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? ReviewedAtUtc { get; set; }
}
