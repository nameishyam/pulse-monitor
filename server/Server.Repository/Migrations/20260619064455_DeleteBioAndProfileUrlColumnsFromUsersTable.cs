using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Repository.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBioAndProfileUrlColumnsFromUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfileUrl",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileUrl",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
