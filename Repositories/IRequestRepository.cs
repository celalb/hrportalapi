using HrPortal.Api.Models;
using HrPortal.Api.Models.Entities;

namespace HrPortal.Api.Repositories;

public interface IRequestRepository
{
    Task CreateAsync(RequestRecord request, CancellationToken cancellationToken);
    Task<IReadOnlyList<RequestRecord>> GetByEmployeeAsync(string employeeId, CancellationToken cancellationToken);
    Task<IReadOnlyList<RequestRecord>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> UpdateStatusAsync(Guid id, RequestStatus status, string? adminNote, DateTime? reviewedAtUtc, CancellationToken cancellationToken);
}
