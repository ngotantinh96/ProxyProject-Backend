using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class addcreatedbyincoreentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WalletHistory",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TransactionHistories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProxyKeys",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProxyKeys",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProxyKeyPlans",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProxyKeyPlans",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProxyHistory",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProxyHistory",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Proxy",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Proxy",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Notifications",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BankAccounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BankAccounts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "qy96BHhg6zCi_X4HXacJF1GTxFj05SF2", "3598010d-0193-49d1-aa59-5b843df8cc7f", "AQAAAAIAAYagAAAAEIqJb0W4qZC5BmgwQEtkc0pqEVwNBNFPoh2lth2PXd8svKwhGLvm+H9npvzKbvCK8A==", "90e81ec3-36be-4ea6-9bd5-5ad024f5ea2f", "WM35mAvSY3gRewCvDGWHqLwbL0D68LO3" });

            migrationBuilder.UpdateData(
                table: "ProxyKeyPlans",
                keyColumn: "Id",
                keyValue: new Guid("2dfa909c-3cd6-494e-9e99-5267b64eb791"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WalletHistory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProxyKeys");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProxyKeys");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProxyKeyPlans");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProxyKeyPlans");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProxyHistory");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProxyHistory");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Proxy");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Proxy");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BankAccounts");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "cAfzfPBBjaQhvirXrDf8BVEur6cekax3", "11028b48-d9e1-437c-8989-a9cc768b4914", "AQAAAAIAAYagAAAAEJxyE1mAD7tgrLZX+GE9XlEeI/fwOhHFxm44ybl4eWDBhX83QqrO2h9ktCVDcuZx8w==", "fe2b94dd-2434-447f-9fab-66839a363e1e", "3iOtvG7eJ221pBMVw1UydY9tyvOdh_G5" });
        }
    }
}
