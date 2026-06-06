namespace HrPortal.Api.Configuration;

public sealed class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";

    public DatabaseProvider Provider { get; set; } = DatabaseProvider.SqlServer;
    public string SqlServerConnectionString { get; set; } = string.Empty;
    public string HanaConnectionString { get; set; } = string.Empty;
    public string HanaProviderInvariantName { get; set; } = "Sap.Data.Hana";
}
