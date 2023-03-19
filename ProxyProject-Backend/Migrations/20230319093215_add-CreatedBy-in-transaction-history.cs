using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class addCreatedByintransactionhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TransactionHistories",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "hyNErwWeuV9crUFo5VRGpRnpglNhEcmi", "ba8eae7a-8d2e-4833-88dc-27506e8661b1", "AQAAAAIAAYagAAAAEMzo9JtxuW1H0Kb2Kv9eja0RDDH+slbAZcgNqZNH4m+APsdwHFSsXlbpjIkbJT4kVw==", "7cde9ad6-2230-484a-9f95-079cbec32abb", "lnQms4w6YPt3gnxuKr1WMA_Y1BhaD2ew" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TransactionHistories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "uJnO5dicLm3BAiaDk9yrhKW9nZyGWaUY", "3d0b139c-00ad-4431-9b3f-76510d9b93ca", "AQAAAAIAAYagAAAAENcxRFxDgFuQ7FDoIrBf1Q+tUChfQ78k2mmvuYWE0RcRzpIAN9HyM6X6iQpnP4fFNw==", "55a4b93c-4352-4e04-ac9a-52a7e05bf44b", "d2ElH0JD7ESpO5Wqk45XwwfgUCujJgfI" });
        }
    }
}
