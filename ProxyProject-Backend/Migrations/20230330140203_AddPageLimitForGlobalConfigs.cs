using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPageLimitForGlobalConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "LimitPage",
            //    table: "GlobalConfiguration",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.UpdateData(
            //    table: "AspNetUsers",
            //    keyColumn: "Id",
            //    keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
            //    columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
            //    values: new object[] { "UpNgbGM1rHMpU3R3qePxCAK9mHeNWDzQ", "b9d0c91c-31ec-4c15-b0ed-9092b27521c4", "AQAAAAIAAYagAAAAEL6adiFeSUsmBS/JIzcNRRGAFnek1JD31nuCsU9zmoPGurY2NbSPcQmACpUTzHGjiw==", "5b06f1d1-158f-4870-9500-2e9df3d50ac8", "yZbwlcDCftrZiQ9F_xOmnry1VqzDrTJh" });

            //migrationBuilder.UpdateData(
            //    table: "GlobalConfiguration",
            //    keyColumn: "Id",
            //    keyValue: new Guid("6bec4da2-3132-4475-934a-c33d4fa9d451"),
            //    column: "LimitPage",
            //    value: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitPage",
                table: "GlobalConfiguration");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "2P1FpfqKn3HUhdVxcJwb0F1Sma4m3JV0", "ad6f7649-8457-4dc4-a1c2-b981031d2afd", "AQAAAAIAAYagAAAAEGQxAntgySzc0z6T37wyswPXxHdyDFwmtDgdJVuO1rZQTviBiDLNI6JY9jsfp+Ghjg==", "aeb466a3-e2bd-4885-8f66-afe0b757937c", "yPRYimpbIb3eeJI0bdLiPD_0yw3EG5CJ" });
        }
    }
}
