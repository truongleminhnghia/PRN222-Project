using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrinkToDoor.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDataBaseV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_item_ingredient_product_ingredient_product_id",
                table: "cart_item");

            migrationBuilder.DropForeignKey(
                name: "FK_cart_item_kit_product_kit_product_id",
                table: "cart_item");

            migrationBuilder.DropColumn(
                name: "manufacturer",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "packing_unit",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "quantity_per_carton",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "quantity_single",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "weight_per_bag",
                table: "ingredient");

            migrationBuilder.RenameColumn(
                name: "quantity_per_packing_unit",
                table: "ingredient",
                newName: "stock_qty");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "ingredient",
                type: "decimal(12,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "ingredient",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "ingredient",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "ingredient",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "cost",
                table: "ingredient",
                type: "decimal(12,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "default_packaging_option_id",
                table: "ingredient",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "supplier_id",
                table: "ingredient",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "kit_product_id",
                table: "cart_item",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "ingredient_product_id",
                table: "cart_item",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "packagin_options",
                columns: table => new
                {
                    packaging_option_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ingredient_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    label = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    size = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    unit = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    cost = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    stock_qty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_packagin_options", x => x.packaging_option_id);
                    table.ForeignKey(
                        name: "FK_packagin_options_ingredient_ingredient_id",
                        column: x => x.ingredient_id,
                        principalTable: "ingredient",
                        principalColumn: "ingredient_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    supplier_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    contact_person = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplier", x => x.supplier_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_default_packaging_option_id",
                table: "ingredient",
                column: "default_packaging_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_supplier_id",
                table: "ingredient",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_packagin_options_ingredient_id",
                table: "packagin_options",
                column: "ingredient_id");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_item_ingredient_product_ingredient_product_id",
                table: "cart_item",
                column: "ingredient_product_id",
                principalTable: "ingredient_product",
                principalColumn: "ingredient_product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_item_kit_product_kit_product_id",
                table: "cart_item",
                column: "kit_product_id",
                principalTable: "kit_product",
                principalColumn: "kit_product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ingredient_packagin_options_default_packaging_option_id",
                table: "ingredient",
                column: "default_packaging_option_id",
                principalTable: "packagin_options",
                principalColumn: "packaging_option_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ingredient_supplier_supplier_id",
                table: "ingredient",
                column: "supplier_id",
                principalTable: "supplier",
                principalColumn: "supplier_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_item_ingredient_product_ingredient_product_id",
                table: "cart_item");

            migrationBuilder.DropForeignKey(
                name: "FK_cart_item_kit_product_kit_product_id",
                table: "cart_item");

            migrationBuilder.DropForeignKey(
                name: "FK_ingredient_packagin_options_default_packaging_option_id",
                table: "ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_ingredient_supplier_supplier_id",
                table: "ingredient");

            migrationBuilder.DropTable(
                name: "packagin_options");

            migrationBuilder.DropTable(
                name: "supplier");

            migrationBuilder.DropIndex(
                name: "IX_ingredient_default_packaging_option_id",
                table: "ingredient");

            migrationBuilder.DropIndex(
                name: "IX_ingredient_supplier_id",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "code",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "cost",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "default_packaging_option_id",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "supplier_id",
                table: "ingredient");

            migrationBuilder.RenameColumn(
                name: "stock_qty",
                table: "ingredient",
                newName: "quantity_per_packing_unit");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "ingredient",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,2)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "ingredient",
                type: "varchar(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ingredient",
                keyColumn: "description",
                keyValue: null,
                column: "description",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "ingredient",
                type: "varchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "manufacturer",
                table: "ingredient",
                type: "varchar(300)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "packing_unit",
                table: "ingredient",
                type: "JSON",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "quantity_per_carton",
                table: "ingredient",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "quantity_single",
                table: "ingredient",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "weight_per_bag",
                table: "ingredient",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<Guid>(
                name: "kit_product_id",
                table: "cart_item",
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
                table: "cart_item",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_item_ingredient_product_ingredient_product_id",
                table: "cart_item",
                column: "ingredient_product_id",
                principalTable: "ingredient_product",
                principalColumn: "ingredient_product_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cart_item_kit_product_kit_product_id",
                table: "cart_item",
                column: "kit_product_id",
                principalTable: "kit_product",
                principalColumn: "kit_product_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
