using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class CartModelAndJoins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepRecipes_Steps_StepId",
                table: "StepRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_AspNetUsers_UserId",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Steps",
                table: "Steps");

            migrationBuilder.RenameTable(
                name: "Steps",
                newName: "Step");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_UserId",
                table: "Step",
                newName: "IX_Step_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Step",
                table: "Step",
                column: "StepId");

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KrogerItemName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    KrogerItemSize = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    KrogerUPC = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    KrogerAisle = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    KrogerImgLink = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    KrogerCost = table.Column<float>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Cart_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartRecipe",
                columns: table => new
                {
                    CartRecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRecipe", x => x.CartRecipeId);
                    table.ForeignKey(
                        name: "FK_CartRecipe_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartRecipe_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartRecipe_CartId",
                table: "CartRecipe",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartRecipe_RecipeId",
                table: "CartRecipe",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Step_AspNetUsers_UserId",
                table: "Step",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepRecipes_Step_StepId",
                table: "StepRecipes",
                column: "StepId",
                principalTable: "Step",
                principalColumn: "StepId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Step_AspNetUsers_UserId",
                table: "Step");

            migrationBuilder.DropForeignKey(
                name: "FK_StepRecipes_Step_StepId",
                table: "StepRecipes");

            migrationBuilder.DropTable(
                name: "CartRecipe");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Step",
                table: "Step");

            migrationBuilder.RenameTable(
                name: "Step",
                newName: "Steps");

            migrationBuilder.RenameIndex(
                name: "IX_Step_UserId",
                table: "Steps",
                newName: "IX_Steps_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Steps",
                table: "Steps",
                column: "StepId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepRecipes_Steps_StepId",
                table: "StepRecipes",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "StepId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_AspNetUsers_UserId",
                table: "Steps",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
