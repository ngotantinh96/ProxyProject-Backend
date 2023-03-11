using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddProxyTableAndCountryCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ProxyKeyPlans",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Proxy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Proxy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartUsingTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndUsingTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ProxyKeyPlanId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proxy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proxy_ProxyKeyPlans_ProxyKeyPlanId",
                        column: x => x.ProxyKeyPlanId,
                        principalTable: "ProxyKeyPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "LAhqovoxU2TD_3ASXPKrxd-740eB8h8p", "58034806-e5db-48b9-a4c7-663dcc072265", "AQAAAAIAAYagAAAAELRTNjJjWo5lFJGcUM8ldxc/OhW4XA9B0sVr8rAEoa9bnItw7h8S5fo+fWVMf++UIg==", "35c8f387-32c3-4982-aeb5-586f0a69c0d8", "Es32LGlvzrP0CL966AioUP4STkNAJO11" });

            migrationBuilder.UpdateData(
                table: "ProxyKeyPlans",
                keyColumn: "Id",
                keyValue: new Guid("2dfa909c-3cd6-494e-9e99-5267b64eb791"),
                column: "Code",
                value: "VN");

            migrationBuilder.CreateIndex(
                name: "IX_Proxy_ProxyKeyPlanId",
                table: "Proxy",
                column: "ProxyKeyPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Proxy");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ProxyKeyPlans");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "Pb7GcYtirIiFPpMBhtHGQIQ76Wp-IIrB", "d5c69160-e929-4763-998c-622b8c167fa0", "AQAAAAIAAYagAAAAEAi327nu0vXnd9Lfjq8ByOHAd0u31a7r4XQTs9yo+rTPwL6vsyFSasOkGzOZVFpRUA==", "abd88503-97f0-4118-8981-88c5c6b9ec4f", "QiD0S_XYpQT-UAfz61G86s8dN0GgwzY9" });
        }
    }
}
