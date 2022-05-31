using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class RenameIngredientProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountForTasty",
                table: "Ingredients",
                newName: "CountForAPIRecipe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountForAPIRecipe",
                table: "Ingredients",
                newName: "CountForTasty");
        }
    }
}
