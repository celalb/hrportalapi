using System.Data;
using HrPortal.Api.Models;
using HrPortal.Api.Models.Entities;
using Microsoft.Data.SqlClient;

namespace HrPortal.Api.Repositories;

public sealed class SqlServerRequestRepository : IRequestRepository
{
    private readonly string _connectionString;

    public SqlServerRequestRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task CreateAsync(RequestRecord request, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO Requests
            (Id, EmployeeId, Type, Title, PayloadJson, Status, AdminNote, CreatedAtUtc, ReviewedAtUtc)
            VALUES (@Id, @EmployeeId, @Type, @Title, @PayloadJson, @Status, @AdminNote, @CreatedAtUtc, @ReviewedAtUtc);
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", request.Id);
        command.Parameters.AddWithValue("@EmployeeId", request.EmployeeId);
        command.Parameters.AddWithValue("@Type", (int)request.Type);
        command.Parameters.AddWithValue("@Title", request.Title);
        command.Parameters.AddWithValue("@PayloadJson", request.PayloadJson);
        command.Parameters.AddWithValue("@Status", (int)request.Status);
        command.Parameters.AddWithValue("@AdminNote", (object?)request.AdminNote ?? DBNull.Value);
        command.Parameters.AddWithValue("@CreatedAtUtc", request.CreatedAtUtc);
        command.Parameters.AddWithValue("@ReviewedAtUtc", (object?)request.ReviewedAtUtc ?? DBNull.Value);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RequestRecord>> GetByEmployeeAsync(string employeeId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Id, EmployeeId, Type, Title, PayloadJson, Status, AdminNote, CreatedAtUtc, ReviewedAtUtc
            FROM Requests
            WHERE EmployeeId = @EmployeeId
            ORDER BY CreatedAtUtc DESC;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@EmployeeId", employeeId);

        return await ReadAsync(command, cancellationToken);
    }

    public async Task<IReadOnlyList<RequestRecord>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Id, EmployeeId, Type, Title, PayloadJson, Status, AdminNote, CreatedAtUtc, ReviewedAtUtc
            FROM Requests
            ORDER BY CreatedAtUtc DESC;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        return await ReadAsync(command, cancellationToken);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, RequestStatus status, string? adminNote, DateTime? reviewedAtUtc, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE Requests
            SET Status = @Status,
                AdminNote = @AdminNote,
                ReviewedAtUtc = @ReviewedAtUtc
            WHERE Id = @Id;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Status", (int)status);
        command.Parameters.AddWithValue("@AdminNote", (object?)adminNote ?? DBNull.Value);
        command.Parameters.AddWithValue("@ReviewedAtUtc", (object?)reviewedAtUtc ?? DBNull.Value);

        var rows = await command.ExecuteNonQueryAsync(cancellationToken);
        return rows > 0;
    }

    private static async Task<IReadOnlyList<RequestRecord>> ReadAsync(SqlCommand command, CancellationToken cancellationToken)
    {
        var items = new List<RequestRecord>();
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult, cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            items.Add(new RequestRecord
            {
                Id = reader.GetGuid(0),
                EmployeeId = reader.GetString(1),
                Type = (RequestType)reader.GetInt32(2),
                Title = reader.GetString(3),
                PayloadJson = reader.GetString(4),
                Status = (RequestStatus)reader.GetInt32(5),
                AdminNote = reader.IsDBNull(6) ? null : reader.GetString(6),
                CreatedAtUtc = reader.GetDateTime(7),
                ReviewedAtUtc = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
            });
        }

        return items;
    }
}
