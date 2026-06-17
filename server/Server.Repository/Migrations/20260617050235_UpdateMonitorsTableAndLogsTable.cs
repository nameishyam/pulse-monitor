using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMonitorsTableAndLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HttpStatusCodes",
                table: "Monitors",
                newName: "HttpStatusCode");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextChecked",
                table: "Monitors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "ResponseTime",
                table: "Logs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextChecked",
                table: "Monitors");

            migrationBuilder.RenameColumn(
                name: "HttpStatusCode",
                table: "Monitors",
                newName: "HttpStatusCodes");

            migrationBuilder.AlterColumn<int>(
                name: "ResponseTime",
                table: "Logs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
