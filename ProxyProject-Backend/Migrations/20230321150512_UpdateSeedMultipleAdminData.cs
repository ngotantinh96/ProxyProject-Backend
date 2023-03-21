using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedMultipleAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "YelMuSztciewTuT56RQMjr0tP6fjnJ05", "3e075bf5-f4a6-4166-8ec4-f7a4cf4e0b81", "LOVENCO0410@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEFmSdW0nfy9tHr/jt0foEGLCeM+IEjX7HNZxfsDZ98oUXMylayE5NGrOjgIIKVyWDg==", "27983fa1-5d4d-4741-999b-f5c9252ba926", "HOUOH6qo_D_y9J3f0LGZNhRl3J61HJ_9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "R6WSqeN-oWpp1jJWKWB_v-nDn-7xs2Sg", "3167eae0-570e-41a9-9137-10d9c9fff90e", null, null, "AQAAAAIAAYagAAAAEFa2Pu6YPMa0gP2TzDeFJB48Z7/u1YfsuLLsKmPrpi+xSjUcGyodn7Izr6GHLio0eA==", "375b9dca-13d8-4495-9089-c00d1d6d26d1", "24_X-hZexGFANLsiXlHCI2t_EW5Voobp" });
        }
    }
}
