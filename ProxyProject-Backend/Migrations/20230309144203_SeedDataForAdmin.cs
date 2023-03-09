using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "848e7b9c-5781-48cb-aa51-c2ee49961828", null, "User", "USER" },
                    { "fb2ec114-eb3c-499d-ab9d-ab8cb579c329", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "APIKey", "AccessFailedCount", "Balance", "ConcurrencyStamp", "Email", "EmailConfirmed", "LimitKeysToCreate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TotalDeposited", "TwoFactorEnabled", "UserName", "WalletKey" },
                values: new object[] { "03a35a7f-e8f9-4856-adb3-f7e548dce6b7", "Pb7GcYtirIiFPpMBhtHGQIQ76Wp-IIrB", 0, 0m, "d5c69160-e929-4763-998c-622b8c167fa0", "ngotantinh96@gmail.com", false, 1000000, false, null, null, null, "AQAAAAIAAYagAAAAEAi327nu0vXnd9Lfjq8ByOHAd0u31a7r4XQTs9yo+rTPwL6vsyFSasOkGzOZVFpRUA==", null, false, "abd88503-97f0-4118-8981-88c5c6b9ec4f", 0m, true, "admin", "QiD0S_XYpQT-UAfz61G86s8dN0GgwzY9" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fb2ec114-eb3c-499d-ab9d-ab8cb579c329", "03a35a7f-e8f9-4856-adb3-f7e548dce6b7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "848e7b9c-5781-48cb-aa51-c2ee49961828");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fb2ec114-eb3c-499d-ab9d-ab8cb579c329", "03a35a7f-e8f9-4856-adb3-f7e548dce6b7" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb2ec114-eb3c-499d-ab9d-ab8cb579c329");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7");
        }
    }
}
