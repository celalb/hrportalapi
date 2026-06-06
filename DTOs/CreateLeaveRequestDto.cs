using System.ComponentModel.DataAnnotations;

namespace HrPortal.Api.DTOs;

public sealed class CreateLeaveRequestDto : IValidatableObject
{
    [Required]
    [MaxLength(100)]
    public string LeaveType { get; init; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; init; }

    [Required]
    public DateOnly EndDate { get; init; }

    [MaxLength(1000)]
    public string? Description { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate < StartDate)
        {
            yield return new ValidationResult("EndDate must be greater than or equal to StartDate.", [nameof(EndDate)]);
        }
    }
}
