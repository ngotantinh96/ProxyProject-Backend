using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class addamountintransactionhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TransactionHistories",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "uJnO5dicLm3BAiaDk9yrhKW9nZyGWaUY", "3d0b139c-00ad-4431-9b3f-76510d9b93ca", "AQAAAAIAAYagAAAAENcxRFxDgFuQ7FDoIrBf1Q+tUChfQ78k2mmvuYWE0RcRzpIAN9HyM6X6iQpnP4fFNw==", "55a4b93c-4352-4e04-ac9a-52a7e05bf44b", "d2ElH0JD7ESpO5Wqk45XwwfgUCujJgfI" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TransactionHistories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "CMM3hVWZAKnLcIqGO5zZhbg_3XexHpJ1", "38939643-0a40-4be4-af3c-7a6ae985d2b3", "AQAAAAIAAYagAAAAEArR8t+H96fj8vIb24YtDDAu00S6I2xIeZ//d9BCp+QN+vVbIgG/JcrwfETut0co3g==", "3c71eb65-31a6-417f-8766-1bb557fc5159", "tF6jsZ5bYgY-siBJXuS2JbfKHsHtR0jM" });
        }
    }
}
