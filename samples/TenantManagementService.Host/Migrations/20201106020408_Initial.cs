using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TenantManagementService.Host.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(nullable: false),
                    CreatorId = table.Column<string>(maxLength: 32, nullable: true),
                    LastModificationTime = table.Column<DateTimeOffset>(nullable: true),
                    LastModifierId = table.Column<string>(maxLength: 32, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<string>(maxLength: 32, nullable: true),
                    DeletionTime = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
