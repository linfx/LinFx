using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfPostgresql.Migrations
{
    /// <inheritdoc />
    public partial class ht : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("select create_hypertable('sensor_stat1', 'crt_time', if_not_exists => true);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
