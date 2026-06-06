using HrPortal.Api.DTOs;

namespace HrPortal.Api.Services;

public interface IRequestService
{
    Task<CreateRequestResponseDto> CreateLeaveAsync(string employeeId, CreateLeaveRequestDto dto, CancellationToken cancellationToken);
    Task<CreateRequestResponseDto> CreateWorklogAsync(string employeeId, CreateWorklogDto dto, CancellationToken cancellationToken);
    Task<CreateRequestResponseDto> CreateAdvanceAsync(string employeeId, CreateAdvanceRequestDto dto, CancellationToken cancellationToken);
    Task<IReadOnlyList<RequestItemDto>> GetMyRequestsAsync(string employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<RequestItemDto>> GetAdminRequestsAsync(CancellationToken cancellationToken);
    Task UpdateStatusAsync(Guid id, UpdateRequestStatusDto dto, CancellationToken cancellationToken);
}
