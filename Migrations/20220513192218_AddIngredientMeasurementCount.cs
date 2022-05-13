using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class AddIngredientMeasurementCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Count",
                table: "Ingredients",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Measurement",
                table: "Ingredients",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Measurement",
                table: "Ingredients");
        }
    }
}
