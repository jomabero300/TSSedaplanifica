using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSSedaplanifica.Data.Migrations
{
    public partial class AddProductEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeasureUnits",
                schema: "Gene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NameShort = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasureUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Seda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(140)", maxLength: 140, nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    MeasureUnitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_MeasureUnits_MeasureUnitId",
                        column: x => x.MeasureUnitId,
                        principalSchema: "Gene",
                        principalTable: "MeasureUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                schema: "Seda",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.CategoryId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Seda",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Seda",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeasureUnit_Name",
                schema: "Gene",
                table: "MeasureUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductId",
                schema: "Seda",
                table: "ProductCategories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ Name",
                schema: "Seda",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_MeasureUnitId",
                schema: "Seda",
                table: "Products",
                column: "MeasureUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategories",
                schema: "Seda");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Seda");

            migrationBuilder.DropTable(
                name: "MeasureUnits",
                schema: "Gene");
        }
    }
}
