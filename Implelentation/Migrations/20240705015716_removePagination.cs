using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aqay_apis.Migrations
{
    /// <inheritdoc />
    public partial class removePagination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Users_BrandOwnerId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Subscriptions_SubscriptionId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_WishLists_WishListId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_WishListId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandOwnerId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WishListId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ConsumerId",
                table: "WishLists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConsumerId",
                table: "ShoppingCarts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BrandOwnerId",
                table: "Brands",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_ConsumerId",
                table: "WishLists",
                column: "ConsumerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ConsumerId",
                table: "ShoppingCarts",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandOwnerId",
                table: "Brands",
                column: "BrandOwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Users_BrandOwnerId",
                table: "Brands",
                column: "BrandOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Users_ConsumerId",
                table: "ShoppingCarts",
                column: "ConsumerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Subscriptions_SubscriptionId",
                table: "Users",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WishLists_Users_ConsumerId",
                table: "WishLists",
                column: "ConsumerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Users_BrandOwnerId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Users_ConsumerId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Subscriptions_SubscriptionId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WishLists_Users_ConsumerId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_WishLists_ConsumerId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_ConsumerId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandOwnerId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "ConsumerId",
                table: "WishLists");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WishListId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConsumerId",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "BrandOwnerId",
                table: "Brands",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WishListId",
                table: "Users",
                column: "WishListId",
                unique: true,
                filter: "[WishListId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandOwnerId",
                table: "Brands",
                column: "BrandOwnerId",
                unique: true,
                filter: "[BrandOwnerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Users_BrandOwnerId",
                table: "Brands",
                column: "BrandOwnerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Subscriptions_SubscriptionId",
                table: "Users",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_WishLists_WishListId",
                table: "Users",
                column: "WishListId",
                principalTable: "WishLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
