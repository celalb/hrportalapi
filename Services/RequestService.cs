using System.Text.Json;
using HrPortal.Api.DTOs;
using HrPortal.Api.Infrastructure;
using HrPortal.Api.Models;
using HrPortal.Api.Models.Entities;
using HrPortal.Api.Repositories;

namespace HrPortal.Api.Services;

public sealed class RequestService : IRequestService
{
    private readonly IRequestRepository _requestRepository;

    public RequestService(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public Task<CreateRequestResponseDto> CreateLeaveAsync(string employeeId, CreateLeaveRequestDto dto, CancellationToken cancellationToken)
    {
        var request = BuildRequest(employeeId, RequestType.Leave, $"Leave request: {dto.LeaveType}", dto);
        return CreateAsync(request, cancellationToken);
    }

    public Task<CreateRequestResponseDto> CreateWorklogAsync(string employeeId, CreateWorklogDto dto, CancellationToken cancellationToken)
    {
        var request = BuildRequest(employeeId, RequestType.Worklog, "Worklog request", dto);
        return CreateAsync(request, cancellationToken);
    }

    public Task<CreateRequestResponseDto> CreateAdvanceAsync(string employeeId, CreateAdvanceRequestDto dto, CancellationToken cancellationToken)
    {
        var request = BuildRequest(employeeId, RequestType.Advance, "Advance request", dto);
        return CreateAsync(request, cancellationToken);
    }

    public async Task<IReadOnlyList<RequestItemDto>> GetMyRequestsAsync(string employeeId, CancellationToken cancellationToken)
    {
        var items = await _requestRepository.GetByEmployeeAsync(employeeId, cancellationToken);
        return items.Select(Map).ToList();
    }

    public async Task<IReadOnlyList<RequestItemDto>> GetAdminRequestsAsync(CancellationToken cancellationToken)
    {
        var items = await _requestRepository.GetAllAsync(cancellationToken);
        return items.Select(Map).ToList();
    }

    public async Task UpdateStatusAsync(Guid id, UpdateRequestStatusDto dto, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(dto.Status))
        {
            throw new ApiException(StatusCodes.Status400BadRequest, "Invalid status value.");
        }

        var reviewedAt = dto.Status == RequestStatus.Pending ? (DateTime?)null : DateTime.UtcNow;
        var updated = await _requestRepository.UpdateStatusAsync(id, dto.Status, dto.AdminNote, reviewedAt, cancellationToken);

        if (!updated)
        {
            throw new ApiException(StatusCodes.Status404NotFound, "Request was not found.");
        }
    }

    private async Task<CreateRequestResponseDto> CreateAsync(RequestRecord request, CancellationToken cancellationToken)
    {
        await _requestRepository.CreateAsync(request, cancellationToken);
        return new CreateRequestResponseDto
        {
            Id = request.Id,
            Status = request.Status
        };
    }

    private static RequestRecord BuildRequest<TPayload>(string employeeId, RequestType requestType, string title, TPayload payload)
    {
        return new RequestRecord
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            Type = requestType,
            Title = title,
            PayloadJson = JsonSerializer.Serialize(payload),
            Status = RequestStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    private static RequestItemDto Map(RequestRecord item)
    {
        return new RequestItemDto
        {
            Id = item.Id,
            EmployeeId = item.EmployeeId,
            Type = item.Type,
            Title = item.Title,
            Status = item.Status,
            AdminNote = item.AdminNote,
            CreatedAtUtc = item.CreatedAtUtc,
            ReviewedAtUtc = item.ReviewedAtUtc,
            Payload = string.IsNullOrWhiteSpace(item.PayloadJson)
                ? null
                : JsonSerializer.Deserialize<object>(item.PayloadJson)
        };
    }
}
