CREATE TABLE Requests (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    EmployeeId NVARCHAR(100) NOT NULL,
    Type INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    PayloadJson NVARCHAR(MAX) NOT NULL,
    Status INT NOT NULL,
    AdminNote NVARCHAR(1000) NULL,
    CreatedAtUtc DATETIME2 NOT NULL,
    ReviewedAtUtc DATETIME2 NULL
);

CREATE INDEX IX_Requests_EmployeeId_CreatedAtUtc ON Requests (EmployeeId, CreatedAtUtc DESC);
