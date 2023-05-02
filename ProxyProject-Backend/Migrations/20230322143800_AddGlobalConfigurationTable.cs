using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddGlobalConfigurationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "GlobalConfiguration",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
            //        TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
            //        CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        CreatedBy = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_GlobalConfiguration", x => x.Id);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.UpdateData(
            //    table: "AspNetUsers",
            //    keyColumn: "Id",
            //    keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
            //    columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
            //    values: new object[] { "2P1FpfqKn3HUhdVxcJwb0F1Sma4m3JV0", "ad6f7649-8457-4dc4-a1c2-b981031d2afd", "AQAAAAIAAYagAAAAEGQxAntgySzc0z6T37wyswPXxHdyDFwmtDgdJVuO1rZQTviBiDLNI6JY9jsfp+Ghjg==", "aeb466a3-e2bd-4885-8f66-afe0b757937c", "yPRYimpbIb3eeJI0bdLiPD_0yw3EG5CJ" });

            //migrationBuilder.InsertData(
            //    table: "GlobalConfiguration",
            //    columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDeleted", "TwoFactorEnabled" },
            //    values: new object[] { new Guid("6bec4da2-3132-4475-934a-c33d4fa9d451"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "GlobalConfiguration");

            //migrationBuilder.UpdateData(
            //    table: "AspNetUsers",
            //    keyColumn: "Id",
            //    keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
            //    columns: new[] { "APIKey", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletKey" },
            //    values: new object[] { "YelMuSztciewTuT56RQMjr0tP6fjnJ05", "3e075bf5-f4a6-4166-8ec4-f7a4cf4e0b81", "AQAAAAIAAYagAAAAEFmSdW0nfy9tHr/jt0foEGLCeM+IEjX7HNZxfsDZ98oUXMylayE5NGrOjgIIKVyWDg==", "27983fa1-5d4d-4741-999b-f5c9252ba926", "HOUOH6qo_D_y9J3f0LGZNhRl3J61HJ_9" });
        }
    }
}
