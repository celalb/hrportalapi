using System.ComponentModel.DataAnnotations;

namespace HrPortal.Api.DTOs;

public sealed class CreateWorklogDto
{
    [Required]
    public DateOnly WorkDate { get; init; }

    [Range(0.1, 24)]
    public decimal Hours { get; init; }

    [Required]
    [MaxLength(1000)]
    public string Reason { get; init; } = string.Empty;
}
