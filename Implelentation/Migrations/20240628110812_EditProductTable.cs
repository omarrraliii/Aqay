using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aqay_apis.Migrations
{
    /// <inheritdoc />
    public partial class EditProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductShoppingCart");

            migrationBuilder.DropTable(
                name: "productVarients");

            migrationBuilder.DropTable(
                name: "ProductWishList");

            migrationBuilder.DropColumn(
                name: "BLUE",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GREEN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RED",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantId",
                table: "WishLists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WishListId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_ProductVariantId",
                table: "WishLists",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShoppingCartId",
                table: "Products",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_WishListId",
                table: "Products",
                column: "WishListId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ShoppingCarts_ShoppingCartId",
                table: "Products",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ShoppingCarts_ShoppingCartId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_WishLists_WishListId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_WishLists_ProductVariants_ProductVariantId",
                table: "WishLists");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_WishLists_ProductVariantId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_Products_ShoppingCartId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_WishListId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductVariantId",
                table: "WishLists");

            migrationBuilder.DropColumn(
                name: "ShoppingCartId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WishListId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "BLUE",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GREEN",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RED",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductShoppingCart",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    ShoppingCartsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductShoppingCart", x => new { x.ProductsId, x.ShoppingCartsId });
                    table.ForeignKey(
                        name: "FK_ProductShoppingCart_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductShoppingCart_ShoppingCarts_ShoppingCartsId",
                        column: x => x.ShoppingCartsId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productVarients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productVarients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductWishList",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    WishListsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWishList", x => new { x.ProductsId, x.WishListsId });
                    table.ForeignKey(
                        name: "FK_ProductWishList_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductWishList_WishLists_WishListsId",
                        column: x => x.WishListsId,
                        principalTable: "WishLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductShoppingCart_ShoppingCartsId",
                table: "ProductShoppingCart",
                column: "ShoppingCartsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWishList_WishListsId",
                table: "ProductWishList",
                column: "WishListsId");
        }
    }
}
