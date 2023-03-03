using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserInfor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LimitKeysToCreate",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitKeysToCreate",
                table: "AspNetUsers");
        }
    }
}
