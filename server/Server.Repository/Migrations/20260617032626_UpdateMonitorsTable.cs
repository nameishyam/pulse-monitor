using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMonitorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HttpStatusCodes",
                table: "Monitors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "HttpMethod",
                table: "Monitors",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HttpMethod",
                table: "Monitors");

            migrationBuilder.AlterColumn<int>(
                name: "HttpStatusCodes",
                table: "Monitors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
