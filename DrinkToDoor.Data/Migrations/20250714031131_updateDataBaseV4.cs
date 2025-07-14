using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrinkToDoor.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDataBaseV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_detail_ingredient_product_ingredient_product_id",
                table: "order_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_order_detail_kit_product_kit_product_id",
                table: "order_detail");

            migrationBuilder.AlterColumn<Guid>(
                name: "kit_product_id",
                table: "order_detail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "ingredient_product_id",
                table: "order_detail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "cart_item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_order_detail_ingredient_product_ingredient_product_id",
                table: "order_detail",
                column: "ingredient_product_id",
                principalTable: "ingredient_product",
                principalColumn: "ingredient_product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_detail_kit_product_kit_product_id",
                table: "order_detail",
                column: "kit_product_id",
                principalTable: "kit_product",
                principalColumn: "kit_product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_detail_ingredient_product_ingredient_product_id",
                table: "order_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_order_detail_kit_product_kit_product_id",
                table: "order_detail");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "cart_item");

            migrationBuilder.AlterColumn<Guid>(
                name: "kit_product_id",
                table: "order_detail",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "ingredient_product_id",
                table: "order_detail",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_order_detail_ingredient_product_ingredient_product_id",
                table: "order_detail",
                column: "ingredient_product_id",
                principalTable: "ingredient_product",
                principalColumn: "ingredient_product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_detail_kit_product_kit_product_id",
                table: "order_detail",
                column: "kit_product_id",
                principalTable: "kit_product",
                principalColumn: "kit_product_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
