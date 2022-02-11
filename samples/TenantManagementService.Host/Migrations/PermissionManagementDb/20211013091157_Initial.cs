using Microsoft.EntityFrameworkCore.Migrations;

namespace TenantManagementService.Migrations.PermissionManagementDb
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Core_PermissionGrant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    ProviderName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_PermissionGrant", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Core_PermissionGrant");
        }
    }
}
