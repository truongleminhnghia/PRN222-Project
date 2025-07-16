using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrinkToDoor.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDataBaseV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_item_ingredient_product_ingredient_product_id",
                table: "cart_item");

            migrationBuilder.DropForeignKey(
                name: "FK_message_user_sender_id",
                table: "message");

            migrationBuilder.AddColumn<Guid>(
                name: "receiver_id",
                table: "message",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "ingredient_product_id",
                table: "cart_item",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_message_receiver_id",
                table: "message",
                column: "receiver_id");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_item_ingredient_product_ingredient_product_id",
                table: "cart_item",
                column: "ingredient_product_id",
                principalTable: "ingredient_product",
                principalColumn: "ingredient_product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_message_user_receiver_id",
                table: "message",
                column: "receiver_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_message_user_sender_id",
                table: "message",
                column: "sender_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_item_ingredient_product_ingredient_product_id",
                table: "cart_item");

            migrationBuilder.DropForeignKey(
                name: "FK_message_user_receiver_id",
                table: "message");

            migrationBuilder.DropForeignKey(
                name: "FK_message_user_sender_id",
                table: "message");

            migrationBuilder.DropIndex(
                name: "IX_message_receiver_id",
                table: "message");

            migrationBuilder.DropColumn(
                name: "receiver_id",
                table: "message");

            migrationBuilder.AlterColumn<Guid>(
                name: "ingredient_product_id",
                table: "cart_item",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_item_ingredient_product_ingredient_product_id",
                table: "cart_item",
                column: "ingredient_product_id",
                principalTable: "ingredient_product",
                principalColumn: "ingredient_product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_message_user_sender_id",
                table: "message",
                column: "sender_id",
                principalTable: "user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
