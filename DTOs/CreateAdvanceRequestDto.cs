using System.ComponentModel.DataAnnotations;

namespace HrPortal.Api.DTOs;

public sealed class CreateAdvanceRequestDto
{
    [Range(0.01, 1000000000)]
    public decimal Amount { get; init; }

    [Required]
    [MaxLength(3)]
    [MinLength(3)]
    public string Currency { get; init; } = "TRY";

    [Required]
    [MaxLength(1000)]
    public string Reason { get; init; } = string.Empty;
}
