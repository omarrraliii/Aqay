using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aqay_apis.Migrations
{
    public partial class seedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                   table: "Roles",
                   columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                   values: new object[] { Guid.NewGuid().ToString(), "Consumer", "Consumer".ToUpper(), Guid.NewGuid().ToString() }
                   );
            migrationBuilder.InsertData(
                    table: "Roles",
                    columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                    values: new object[] { Guid.NewGuid().ToString(), "Owner", "Owner".ToUpper(), Guid.NewGuid().ToString() }
                    );
            migrationBuilder.InsertData(
                    table: "Roles",
                    columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                    values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() }
                    );
            migrationBuilder.InsertData(
                   table: "Roles",
                   columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                   values: new object[] { Guid.NewGuid().ToString(), "Staff", "Staff".ToUpper(), Guid.NewGuid().ToString() }
                   );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "Role1Id"
            );

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "Role2Id"
            );

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "Role3Id"
            );

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "Role4Id"
            );
        }

    }
}
