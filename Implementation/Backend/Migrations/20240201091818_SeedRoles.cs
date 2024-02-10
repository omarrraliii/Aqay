using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aqay_v2.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                    table: "Roles",
                    columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                    values: new object[] {Guid.NewGuid().ToString(), "Buyer" , "Buyer".ToUpper() , Guid.NewGuid().ToString() },
                    schema: "Security"
                    );
            migrationBuilder.InsertData(
                    table: "Roles",
                    columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                    values: new object[] { Guid.NewGuid().ToString(), "Seller", "Seller".ToUpper(), Guid.NewGuid().ToString() },
                    schema: "Security"
                    );
            migrationBuilder.InsertData(
                    table: "Roles",
                    columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                    values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() },
                    schema: "Security"
                    );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Roles WHERE Name = 'Buyer'");
            migrationBuilder.Sql("DELETE FROM Roles WHERE Name = 'Seller'");
            migrationBuilder.Sql("DELETE FROM Roles WHERE Name = 'Admin'");
        }


    }
}
