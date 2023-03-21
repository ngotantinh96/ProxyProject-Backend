using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProxyProject_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "YbGx3gg62kQgOTnuH0z15J9X23Mdum2N", "3f993a87-e581-4039-b65d-ff06b89f309f", "THHIENS2TH@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEH4D4nUX0XF7GDXWcdH4KCFLRSmHYGHmrPZ8Nclr7q0fNPm24bcK8mAqMklPUAnC7Q==", "fd3a5009-0750-4bc3-991b-91f30c971fb6", "g4iuTdPdlH9QPzqT7lcJq968oqpeOUmI" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                columns: new[] { "APIKey", "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "WalletKey" },
                values: new object[] { "cAfzfPBBjaQhvirXrDf8BVEur6cekax3", "11028b48-d9e1-437c-8989-a9cc768b4914", null, null, "AQAAAAIAAYagAAAAEJxyE1mAD7tgrLZX+GE9XlEeI/fwOhHFxm44ybl4eWDBhX83QqrO2h9ktCVDcuZx8w==", "fe2b94dd-2434-447f-9fab-66839a363e1e", "3iOtvG7eJ221pBMVw1UydY9tyvOdh_G5" });
        }
    }
}
