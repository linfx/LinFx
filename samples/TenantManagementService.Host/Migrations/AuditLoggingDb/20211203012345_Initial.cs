using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TenantManagementService.Host.Migrations.AuditLoggingDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditing_AuditLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ApplicationName = table.Column<string>(type: "TEXT", maxLength: 96, nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    TenantId = table.Column<string>(type: "TEXT", nullable: true),
                    TenantName = table.Column<string>(type: "TEXT", nullable: true),
                    ImpersonatorUserId = table.Column<string>(type: "TEXT", nullable: true),
                    ImpersonatorTenantId = table.Column<string>(type: "TEXT", nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExecutionDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientIpAddress = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    ClientId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    CorrelationId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    BrowserInfo = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    HttpMethod = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Exceptions = table.Column<string>(type: "TEXT", nullable: true),
                    Comments = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    HttpStatusCode = table.Column<int>(type: "INTEGER", nullable: true),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditing_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auditing_AuditLogActions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", nullable: true),
                    AuditLogId = table.Column<string>(type: "TEXT", nullable: false),
                    ServiceName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Parameters = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExecutionDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditing_AuditLogActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditing_AuditLogActions_Auditing_AuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "Auditing_AuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditing_EntityChanges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    AuditLogId = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", nullable: true),
                    ChangeTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ChangeType = table.Column<byte>(type: "INTEGER", nullable: false),
                    EntityTenantId = table.Column<string>(type: "TEXT", nullable: true),
                    EntityId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    EntityTypeFullName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditing_EntityChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditing_EntityChanges_Auditing_AuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "Auditing_AuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditing_EntityPropertyChanges",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", nullable: true),
                    EntityChangeId = table.Column<string>(type: "TEXT", nullable: true),
                    NewValue = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    PropertyTypeFullName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditing_EntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditing_EntityPropertyChanges_Auditing_EntityChanges_EntityChangeId",
                        column: x => x.EntityChangeId,
                        principalTable: "Auditing_EntityChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditing_AuditLogActions_AuditLogId",
                table: "Auditing_AuditLogActions",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditing_AuditLogActions_TenantId_ServiceName_MethodName_ExecutionTime",
                table: "Auditing_AuditLogActions",
                columns: new[] { "TenantId", "ServiceName", "MethodName", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Auditing_AuditLogs_TenantId_ExecutionTime",
                table: "Auditing_AuditLogs",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Auditing_AuditLogs_TenantId_UserId_ExecutionTime",
                table: "Auditing_AuditLogs",
                columns: new[] { "TenantId", "UserId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Auditing_EntityChanges_AuditLogId",
                table: "Auditing_EntityChanges",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Auditing_EntityChanges_TenantId_EntityTypeFullName_EntityId",
                table: "Auditing_EntityChanges",
                columns: new[] { "TenantId", "EntityTypeFullName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_Auditing_EntityPropertyChanges_EntityChangeId",
                table: "Auditing_EntityPropertyChanges",
                column: "EntityChangeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditing_AuditLogActions");

            migrationBuilder.DropTable(
                name: "Auditing_EntityPropertyChanges");

            migrationBuilder.DropTable(
                name: "Auditing_EntityChanges");

            migrationBuilder.DropTable(
                name: "Auditing_AuditLogs");
        }
    }
}
