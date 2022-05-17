using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class UpdateCartDBName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_AspNetUsers_UserId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_CartRecipe_Cart_CartId",
                table: "CartRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_Step_AspNetUsers_UserId",
                table: "Step");

            migrationBuilder.DropForeignKey(
                name: "FK_StepRecipes_Step_StepId",
                table: "StepRecipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Step",
                table: "Step");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart",
                table: "Cart");

            migrationBuilder.RenameTable(
                name: "Step",
                newName: "Steps");

            migrationBuilder.RenameTable(
                name: "Cart",
                newName: "Carts");

            migrationBuilder.RenameIndex(
                name: "IX_Step_UserId",
                table: "Steps",
                newName: "IX_Steps_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_UserId",
                table: "Carts",
                newName: "IX_Carts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Steps",
                table: "Steps",
                column: "StepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartRecipe_Carts_CartId",
                table: "CartRecipe",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AspNetUsers_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartRecipe_Carts_CartId",
                table: "CartRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_AspNetUsers_UserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_StepRecipes_Steps_StepId",
                table: "StepRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Steps_AspNetUsers_UserId",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Steps",
                table: "Steps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.RenameTable(
                name: "Steps",
                newName: "Step");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "Cart");

            migrationBuilder.RenameIndex(
                name: "IX_Steps_UserId",
                table: "Step",
                newName: "IX_Step_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Carts_UserId",
                table: "Cart",
                newName: "IX_Cart_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Step",
                table: "Step",
                column: "StepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart",
                table: "Cart",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_AspNetUsers_UserId",
                table: "Cart",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartRecipe_Cart_CartId",
                table: "CartRecipe",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);

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
    }
}
