using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrinkToDoor.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabaseV5 : Migration
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

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    sender_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    message = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    readed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message", x => x.message_id);
                    table.ForeignKey(
                        name: "FK_message_user_sender_id",
                        column: x => x.sender_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_message_sender_id",
                table: "message",
                column: "sender_id");

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

            migrationBuilder.DropTable(
                name: "message");

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
