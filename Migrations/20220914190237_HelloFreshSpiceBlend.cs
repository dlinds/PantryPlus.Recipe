using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class HelloFreshSpiceBlend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HelloFreshSpiceBlends",
                columns: table => new
                {
                    HelloFreshSpiceBlendId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HelloFreshSpiceBlendName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelloFreshSpiceBlends", x => x.HelloFreshSpiceBlendId);
                });

            migrationBuilder.CreateTable(
                name: "HelloFreshSpiceBlendIngredients",
                columns: table => new
                {
                    HelloFreshSpiceBlendIngredientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    HelloFreshSpiceBlendId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelloFreshSpiceBlendIngredients", x => x.HelloFreshSpiceBlendIngredientId);
                    table.ForeignKey(
                        name: "FK_HelloFreshSpiceBlendIngredients_HelloFreshSpiceBlends_HelloF~",
                        column: x => x.HelloFreshSpiceBlendId,
                        principalTable: "HelloFreshSpiceBlends",
                        principalColumn: "HelloFreshSpiceBlendId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HelloFreshSpiceBlendIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HelloFreshSpiceBlendIngredients_HelloFreshSpiceBlendId",
                table: "HelloFreshSpiceBlendIngredients",
                column: "HelloFreshSpiceBlendId");

            migrationBuilder.CreateIndex(
                name: "IX_HelloFreshSpiceBlendIngredients_IngredientId",
                table: "HelloFreshSpiceBlendIngredients",
                column: "IngredientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelloFreshSpiceBlendIngredients");

            migrationBuilder.DropTable(
                name: "HelloFreshSpiceBlends");
        }
    }
}
