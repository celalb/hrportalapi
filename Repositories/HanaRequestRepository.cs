using System.Data.Common;
using HrPortal.Api.Infrastructure;
using HrPortal.Api.Models;
using HrPortal.Api.Models.Entities;

namespace HrPortal.Api.Repositories;

public sealed class HanaRequestRepository : IRequestRepository
{
    private readonly string _connectionString;
    private readonly string _providerInvariantName;

    public HanaRequestRepository(string connectionString, string providerInvariantName)
    {
        _connectionString = connectionString;
        _providerInvariantName = providerInvariantName;
    }

    public async Task CreateAsync(RequestRecord request, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO "Requests"
            ("Id", "EmployeeId", "Type", "Title", "PayloadJson", "Status", "AdminNote", "CreatedAtUtc", "ReviewedAtUtc")
            VALUES (:Id, :EmployeeId, :Type, :Title, :PayloadJson, :Status, :AdminNote, :CreatedAtUtc, :ReviewedAtUtc)
            """;

        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        AddParameter(command, "Id", request.Id.ToString());
        AddParameter(command, "EmployeeId", request.EmployeeId);
        AddParameter(command, "Type", (int)request.Type);
        AddParameter(command, "Title", request.Title);
        AddParameter(command, "PayloadJson", request.PayloadJson);
        AddParameter(command, "Status", (int)request.Status);
        AddParameter(command, "AdminNote", request.AdminNote);
        AddParameter(command, "CreatedAtUtc", request.CreatedAtUtc);
        AddParameter(command, "ReviewedAtUtc", request.ReviewedAtUtc);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RequestRecord>> GetByEmployeeAsync(string employeeId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT "Id", "EmployeeId", "Type", "Title", "PayloadJson", "Status", "AdminNote", "CreatedAtUtc", "ReviewedAtUtc"
            FROM "Requests"
            WHERE "EmployeeId" = :EmployeeId
            ORDER BY "CreatedAtUtc" DESC
            """;

        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        AddParameter(command, "EmployeeId", employeeId);

        return await ReadAsync(command, cancellationToken);
    }

    public async Task<IReadOnlyList<RequestRecord>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT "Id", "EmployeeId", "Type", "Title", "PayloadJson", "Status", "AdminNote", "CreatedAtUtc", "ReviewedAtUtc"
            FROM "Requests"
            ORDER BY "CreatedAtUtc" DESC
            """;

        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        return await ReadAsync(command, cancellationToken);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, RequestStatus status, string? adminNote, DateTime? reviewedAtUtc, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE "Requests"
            SET "Status" = :Status,
                "AdminNote" = :AdminNote,
                "ReviewedAtUtc" = :ReviewedAtUtc
            WHERE "Id" = :Id
            """;

        await using var connection = CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        AddParameter(command, "Id", id.ToString());
        AddParameter(command, "Status", (int)status);
        AddParameter(command, "AdminNote", adminNote);
        AddParameter(command, "ReviewedAtUtc", reviewedAtUtc);

        var rows = await command.ExecuteNonQueryAsync(cancellationToken);
        return rows > 0;
    }

    private DbConnection CreateConnection()
    {
        try
        {
            var factory = DbProviderFactories.GetFactory(_providerInvariantName);
            var connection = factory.CreateConnection() ?? throw new ApiException(StatusCodes.Status500InternalServerError, "HANA provider could not create a connection.");
            connection.ConnectionString = _connectionString;
            return connection;
        }
        catch (ArgumentException)
        {
            throw new ApiException(StatusCodes.Status500InternalServerError,
                $"HANA provider '{_providerInvariantName}' is not registered. Install and register SAP HANA ADO.NET provider.");
        }
    }

    private static void AddParameter(DbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }

    private static async Task<IReadOnlyList<RequestRecord>> ReadAsync(DbCommand command, CancellationToken cancellationToken)
    {
        var items = new List<RequestRecord>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var rawId = reader.GetValue(0).ToString() ?? string.Empty;
            items.Add(new RequestRecord
            {
                Id = Guid.TryParse(rawId, out var guid) ? guid : Guid.Empty,
                EmployeeId = reader.GetString(1),
                Type = (RequestType)Convert.ToInt32(reader.GetValue(2)),
                Title = reader.GetString(3),
                PayloadJson = reader.GetString(4),
                Status = (RequestStatus)Convert.ToInt32(reader.GetValue(5)),
                AdminNote = reader.IsDBNull(6) ? null : reader.GetString(6),
                CreatedAtUtc = Convert.ToDateTime(reader.GetValue(7)),
                ReviewedAtUtc = reader.IsDBNull(8) ? null : Convert.ToDateTime(reader.GetValue(8))
            });
        }

        return items;
    }
}
