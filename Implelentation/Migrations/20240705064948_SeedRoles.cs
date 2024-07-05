using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aqay_apis.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Consumer", "CONSUMER", Guid.NewGuid().ToString() }
            );
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Owner", "OWNER", Guid.NewGuid().ToString() }
            );
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Admin", "ADMIN", Guid.NewGuid().ToString() }
            );
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Staff", "STAFF", Guid.NewGuid().ToString() }
            );
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { Guid.NewGuid().ToString(), "Guest", "GUEST", Guid.NewGuid().ToString() }
            );
        }
        /// <inheritdoc />

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Consumer"
            );

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Owner"
            );

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Admin"
            );

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Staff"
            );

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Guest"
            );
        }
    }
}
