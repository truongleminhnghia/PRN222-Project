using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrinkToDoor.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_kit_category_category_id",
                table: "kit");

            migrationBuilder.DropIndex(
                name: "IX_kit_category_id",
                table: "kit");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "kit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "category_id",
                table: "kit",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_kit_category_id",
                table: "kit",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_kit_category_category_id",
                table: "kit",
                column: "category_id",
                principalTable: "category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
