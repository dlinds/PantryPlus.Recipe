using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class AddHelloFreshAPIValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HelloFreshTokenValue",
                table: "HelloFreshToken",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HelloFreshTokenValue",
                table: "HelloFreshToken");
        }
    }
}
