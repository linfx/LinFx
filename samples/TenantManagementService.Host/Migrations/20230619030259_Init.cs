﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantManagementService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ExtraProperties = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatorId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LastModifierId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    DeleterId = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
