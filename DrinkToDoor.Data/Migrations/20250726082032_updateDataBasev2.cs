using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrinkToDoor.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDataBasev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ingredient_supplier_supplier_id",
                table: "ingredient");

            migrationBuilder.DropTable(
                name: "supplier");

            migrationBuilder.DropIndex(
                name: "IX_ingredient_supplier_id",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "supplier_id",
                table: "ingredient");

            migrationBuilder.AddColumn<string>(
                name: "origin",
                table: "ingredient",
                type: "varchar(300)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "trademark",
                table: "ingredient",
                type: "varchar(300)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "origin",
                table: "ingredient");

            migrationBuilder.DropColumn(
                name: "trademark",
                table: "ingredient");

            migrationBuilder.AddColumn<Guid>(
                name: "supplier_id",
                table: "ingredient",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    supplier_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    contact_person = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplier", x => x.supplier_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_supplier_id",
                table: "ingredient",
                column: "supplier_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ingredient_supplier_supplier_id",
                table: "ingredient",
                column: "supplier_id",
                principalTable: "supplier",
                principalColumn: "supplier_id");
        }
    }
}
