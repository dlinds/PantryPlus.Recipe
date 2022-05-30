using Microsoft.EntityFrameworkCore.Migrations;

namespace PantryPlusRecipe.Migrations
{
    public partial class RenameDBSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HelloFreshToken_AspNetUsers_UserId",
                table: "HelloFreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HelloFreshToken",
                table: "HelloFreshToken");

            migrationBuilder.RenameTable(
                name: "HelloFreshToken",
                newName: "HelloFreshTokens");

            migrationBuilder.RenameIndex(
                name: "IX_HelloFreshToken_UserId",
                table: "HelloFreshTokens",
                newName: "IX_HelloFreshTokens_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HelloFreshTokens",
                table: "HelloFreshTokens",
                column: "HelloFreshTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_HelloFreshTokens_AspNetUsers_UserId",
                table: "HelloFreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HelloFreshTokens_AspNetUsers_UserId",
                table: "HelloFreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HelloFreshTokens",
                table: "HelloFreshTokens");

            migrationBuilder.RenameTable(
                name: "HelloFreshTokens",
                newName: "HelloFreshToken");

            migrationBuilder.RenameIndex(
                name: "IX_HelloFreshTokens_UserId",
                table: "HelloFreshToken",
                newName: "IX_HelloFreshToken_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HelloFreshToken",
                table: "HelloFreshToken",
                column: "HelloFreshTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_HelloFreshToken_AspNetUsers_UserId",
                table: "HelloFreshToken",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
