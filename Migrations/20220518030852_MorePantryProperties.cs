using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class MorePantryProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Pantry");

            migrationBuilder.RenameColumn(
                name: "WeightCount",
                table: "Pantry",
                newName: "KrogerItemSize");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Pantry",
                newName: "KrogerItemName");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Pantry",
                newName: "KrogerImgLink");

            migrationBuilder.AddColumn<int>(
                name: "ItemCount",
                table: "Pantry",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KrogerAisle",
                table: "Pantry",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KrogerCategory",
                table: "Pantry",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "KrogerCost",
                table: "Pantry",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemCount",
                table: "Pantry");

            migrationBuilder.DropColumn(
                name: "KrogerAisle",
                table: "Pantry");

            migrationBuilder.DropColumn(
                name: "KrogerCategory",
                table: "Pantry");

            migrationBuilder.DropColumn(
                name: "KrogerCost",
                table: "Pantry");

            migrationBuilder.RenameColumn(
                name: "KrogerItemSize",
                table: "Pantry",
                newName: "WeightCount");

            migrationBuilder.RenameColumn(
                name: "KrogerItemName",
                table: "Pantry",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "KrogerImgLink",
                table: "Pantry",
                newName: "Category");

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Pantry",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
