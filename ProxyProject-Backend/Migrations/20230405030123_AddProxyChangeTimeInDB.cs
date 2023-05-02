using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddProxyChangeTimeInDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<double>(
            //    name: "ProxyChangeTime",
            //    table: "GlobalConfiguration",
            //    type: "double",
            //    nullable: false,
            //    defaultValue: 0.0);

            //migrationBuilder.UpdateData(
            //    table: "AspNetUsers",
            //    keyColumn: "Id",
            //    keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
            //    columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
            //    values: new object[] { "_r5ZlFIpUdi114BP2wjrJOVta8RBCkCE", "445f1cdf-bf0f-4395-9d0c-db8af8fb1ffd", "AQAAAAIAAYagAAAAEOOiretXqxenLguFoIyOADAaJYBJjQh9K1TFzqTvOPj60VQLe6TJf+j72ABsLB7Ueg==", "140f1da2-0110-43ba-abb3-0b215dfcb7e8", "deFtZqMZxs5hgwpRY1g38wZfMQ-povw8" });

            //migrationBuilder.UpdateData(
            //    table: "GlobalConfiguration",
            //    keyColumn: "Id",
            //    keyValue: new Guid("6bec4da2-3132-4475-934a-c33d4fa9d451"),
            //    column: "ProxyChangeTime",
            //    value: 120.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProxyChangeTime",
                table: "GlobalConfiguration");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "UpNgbGM1rHMpU3R3qePxCAK9mHeNWDzQ", "b9d0c91c-31ec-4c15-b0ed-9092b27521c4", "AQAAAAIAAYagAAAAEL6adiFeSUsmBS/JIzcNRRGAFnek1JD31nuCsU9zmoPGurY2NbSPcQmACpUTzHGjiw==", "5b06f1d1-158f-4870-9500-2e9df3d50ac8", "yZbwlcDCftrZiQ9F_xOmnry1VqzDrTJh" });
        }
    }
}
