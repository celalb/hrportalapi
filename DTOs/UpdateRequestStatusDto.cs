using System.ComponentModel.DataAnnotations;
using HrPortal.Api.Models;

namespace HrPortal.Api.DTOs;

public sealed class UpdateRequestStatusDto
{
    [Required]
    public RequestStatus Status { get; init; }

    [MaxLength(1000)]
    public string? AdminNote { get; init; }
}
