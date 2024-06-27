using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aqay_apis.Migrations
{
    /// <inheritdoc />
    public partial class AddingTitleToReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Describtion",
                table: "Products",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "Describtion");
        }
    }
}
