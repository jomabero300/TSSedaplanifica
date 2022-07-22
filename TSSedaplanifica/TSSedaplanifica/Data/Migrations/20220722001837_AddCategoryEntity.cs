using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSSedaplanifica.Data.Migrations
{
    public partial class AddCategoryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Admi");

            migrationBuilder.EnsureSchema(
                name: "Seda");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "Admi");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "Admi");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "Admi");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "Admi");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "Admi");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "Admi");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "Admi");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "Seda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTypes",
                schema: "Seda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTypeDers",
                schema: "Seda",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryTypeId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypeDers", x => new { x.CategoryId, x.CategoryTypeId });
                    table.ForeignKey(
                        name: "FK_CategoryTypeDers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Seda",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryTypeDers_CategoryTypes_CategoryTypeId",
                        column: x => x.CategoryTypeId,
                        principalSchema: "Seda",
                        principalTable: "CategoryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_ Name",
                schema: "Seda",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTypeDers_CategoryTypeId",
                schema: "Seda",
                table: "CategoryTypeDers",
                column: "CategoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryType_ Name",
                schema: "Seda",
                table: "CategoryTypes",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTypeDers",
                schema: "Seda");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "Seda");

            migrationBuilder.DropTable(
                name: "CategoryTypes",
                schema: "Seda");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "Admi",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "Admi",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "Admi",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "Admi",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "Admi",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "Admi",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "Admi",
                newName: "AspNetRoleClaims");
        }
    }
}
