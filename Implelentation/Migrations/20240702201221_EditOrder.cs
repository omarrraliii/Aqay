using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aqay_apis.Migrations
{
    /// <inheritdoc />
    public partial class EditOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Users_ConsumerId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_ConsumerId",
                table: "ShoppingCarts");

            migrationBuilder.AlterColumn<string>(
                name: "ConsumerId",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ConsumerId",
                table: "ShoppingCarts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ConsumerId",
                table: "ShoppingCarts",
                column: "ConsumerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Users_ConsumerId",
                table: "ShoppingCarts",
                column: "ConsumerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
