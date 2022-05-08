using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class FixJoinTableForRecipeSteps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeStepRecipes_Recipes_RecipeId",
                table: "RecipeStepRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeStepRecipes_Steps_StepId",
                table: "RecipeStepRecipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeStepRecipes",
                table: "RecipeStepRecipes");

            migrationBuilder.RenameTable(
                name: "RecipeStepRecipes",
                newName: "StepRecipes");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeStepRecipes_StepId",
                table: "StepRecipes",
                newName: "IX_StepRecipes_StepId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeStepRecipes_RecipeId",
                table: "StepRecipes",
                newName: "IX_StepRecipes_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StepRecipes",
                table: "StepRecipes",
                column: "StepRecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepRecipes_Recipes_RecipeId",
                table: "StepRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StepRecipes_Steps_StepId",
                table: "StepRecipes",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "StepId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepRecipes_Recipes_RecipeId",
                table: "StepRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_StepRecipes_Steps_StepId",
                table: "StepRecipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StepRecipes",
                table: "StepRecipes");

            migrationBuilder.RenameTable(
                name: "StepRecipes",
                newName: "RecipeStepRecipes");

            migrationBuilder.RenameIndex(
                name: "IX_StepRecipes_StepId",
                table: "RecipeStepRecipes",
                newName: "IX_RecipeStepRecipes_StepId");

            migrationBuilder.RenameIndex(
                name: "IX_StepRecipes_RecipeId",
                table: "RecipeStepRecipes",
                newName: "IX_RecipeStepRecipes_RecipeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeStepRecipes",
                table: "RecipeStepRecipes",
                column: "StepRecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeStepRecipes_Recipes_RecipeId",
                table: "RecipeStepRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeStepRecipes_Steps_StepId",
                table: "RecipeStepRecipes",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "StepId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
