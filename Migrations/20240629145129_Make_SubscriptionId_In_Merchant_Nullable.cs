using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aqay_apis.Migrations
{
    /// <inheritdoc />
    public partial class Make_SubscriptionId_In_Merchant_Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Subscriptions_SubscriptionId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Subscriptions_SubscriptionId",
                table: "Users",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Subscriptions_SubscriptionId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Subscriptions_SubscriptionId",
                table: "Users",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
