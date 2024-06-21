using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace aqay_apis.Migrations
{
    /// <inheritdoc />
    public partial class seedPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Describtion", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Monthly subscription", "Monthly", 9.9900000000000002 },
                    { 2, "Quarterly subscription", "Quarterly", 27.989999999999998 },
                    { 3, "Yearly subscription", "Yearly", 99.989999999999995 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
