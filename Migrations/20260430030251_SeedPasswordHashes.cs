using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaPlus.CinemaWebApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedPasswordHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "PBKDF2-SHA256$100000$Q2luZW1hUGx1c0FkbWluU2VlZFNhbHQyMDI2$QH4wb86t1iJR1VcbnplnhpgMk7Q4ROIyNmqOM6m5+Sg=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "PBKDF2-SHA256$100000$Q2luZW1hUGx1c0NsaWVudFNlZWRTYWx0MjAyNg==$Np9+ovw8byxBN8DeP7YEN+iDXUE+4HGEA0+05IxctiE=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "DEMO_PASSWORD_HASH_admin123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "DEMO_PASSWORD_HASH_client123");
        }
    }
}
