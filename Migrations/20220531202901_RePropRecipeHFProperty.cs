using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class RePropRecipeHFProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HelloFreshId",
                table: "Recipes",
                newName: "APIRecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "APIRecipeId",
                table: "Recipes",
                newName: "HelloFreshId");
        }
    }
}
