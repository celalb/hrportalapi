CREATE COLUMN TABLE "Requests" (
    "Id" NVARCHAR(36) NOT NULL,
    "EmployeeId" NVARCHAR(100) NOT NULL,
    "Type" INTEGER NOT NULL,
    "Title" NVARCHAR(200) NOT NULL,
    "PayloadJson" NCLOB NOT NULL,
    "Status" INTEGER NOT NULL,
    "AdminNote" NVARCHAR(1000),
    "CreatedAtUtc" TIMESTAMP NOT NULL,
    "ReviewedAtUtc" TIMESTAMP,
    PRIMARY KEY ("Id")
);

CREATE INDEX "IX_Requests_EmployeeId_CreatedAtUtc" ON "Requests" ("EmployeeId", "CreatedAtUtc");
