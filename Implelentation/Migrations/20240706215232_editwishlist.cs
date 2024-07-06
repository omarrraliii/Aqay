using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aqay_apis.Migrations
{
    /// <inheritdoc />
    public partial class editwishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_WishLists_WishListId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_WishLists_ProductVariants_ProductVariantId",
                table: "WishLists");

            migrationBuilder.DropForeignKey(
                name: "FK_WishLists_Users_ConsumerId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_WishLists_ConsumerId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_WishLists_ProductVariantId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_Products_WishListId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductVariantId",
                table: "WishLists");

            migrationBuilder.DropColumn(
                name: "WishListId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ConsumerId",
                table: "WishLists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "WishListId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductVariantWishList",
                columns: table => new
                {
                    ProductsVariantsId = table.Column<int>(type: "int", nullable: false),
                    WishListsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantWishList", x => new { x.ProductsVariantsId, x.WishListsId });
                    table.ForeignKey(
                        name: "FK_ProductVariantWishList_ProductVariants_ProductsVariantsId",
                        column: x => x.ProductsVariantsId,
                        principalTable: "ProductVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariantWishList_WishLists_WishListsId",
                        column: x => x.WishListsId,
                        principalTable: "WishLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantWishList_WishListsId",
                table: "ProductVariantWishList",
                column: "WishListsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariantWishList");

            migrationBuilder.DropColumn(
                name: "WishListId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "ConsumerId",
                table: "WishLists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantId",
                table: "WishLists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WishListId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_ConsumerId",
                table: "WishLists",
                column: "ConsumerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_ProductVariantId",
                table: "WishLists",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_WishListId",
                table: "Products",
                column: "WishListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_WishLists_WishListId",
                table: "Products",
                column: "WishListId",
                principalTable: "WishLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WishLists_ProductVariants_ProductVariantId",
                table: "WishLists",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WishLists_Users_ConsumerId",
                table: "WishLists",
                column: "ConsumerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
