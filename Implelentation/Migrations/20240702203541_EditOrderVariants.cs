using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aqay_apis.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductVariantId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductVariantId",
                table: "Orders");
        }
    }
}
