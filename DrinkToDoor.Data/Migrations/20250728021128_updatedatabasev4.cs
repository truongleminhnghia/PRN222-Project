using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrinkToDoor.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabasev4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "kit");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "kit");

            migrationBuilder.AddColumn<DateTime>(
                name: "status_changedAt",
                table: "order_infor",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status_changedAt",
                table: "order_infor");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "kit",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "kit",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
