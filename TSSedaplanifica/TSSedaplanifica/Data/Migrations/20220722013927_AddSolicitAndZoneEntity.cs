using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSSedaplanifica.Data.Migrations
{
    public partial class AddSolicitAndZoneEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Admi",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Admi",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTypeDers_Categories_CategoryId",
                schema: "Seda",
                table: "CategoryTypeDers");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTypeDers_CategoryTypes_CategoryTypeId",
                schema: "Seda",
                table: "CategoryTypeDers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                schema: "Seda",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                schema: "Seda",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_MeasureUnits_MeasureUnitId",
                schema: "Seda",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Cities_CityId",
                schema: "Seda",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Zones_ZoneId",
                schema: "Seda",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolUsers_AspNetUsers_ApplicationUserId",
                schema: "Seda",
                table: "SchoolUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolUsers_Schools_SchoolId",
                schema: "Seda",
                table: "SchoolUsers");

            migrationBuilder.CreateTable(
                name: "SolicitState",
                schema: "Seda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Solicit",
                schema: "Seda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    DateOfSolicit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    SolicitStatesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicit_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "Seda",
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicit_SolicitState_SolicitStatesId",
                        column: x => x.SolicitStatesId,
                        principalSchema: "Seda",
                        principalTable: "SolicitState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitDetails",
                schema: "Seda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    DirectorQuantity = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    PlannerQuantity = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    DeliveredQuantity = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolicitDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Seda",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitDetails_Solicit_SolicitId",
                        column: x => x.SolicitId,
                        principalSchema: "Seda",
                        principalTable: "Solicit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zone_Name",
                schema: "Gene",
                table: "Zones",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicit_SchoolId",
                schema: "Seda",
                table: "Solicit",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicit_SolicitStatesId",
                schema: "Seda",
                table: "Solicit",
                column: "SolicitStatesId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitDetail_Solicit_Product_ Id",
                schema: "Seda",
                table: "SolicitDetails",
                columns: new[] { "SolicitId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitDetails_ProductId",
                schema: "Seda",
                table: "SolicitDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitState_ Name",
                schema: "Seda",
                table: "SolicitState",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Admi",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "Admi",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Admi",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "Admi",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTypeDers_Categories_CategoryId",
                schema: "Seda",
                table: "CategoryTypeDers",
                column: "CategoryId",
                principalSchema: "Seda",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTypeDers_CategoryTypes_CategoryTypeId",
                schema: "Seda",
                table: "CategoryTypeDers",
                column: "CategoryTypeId",
                principalSchema: "Seda",
                principalTable: "CategoryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                schema: "Seda",
                table: "ProductCategories",
                column: "CategoryId",
                principalSchema: "Seda",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                schema: "Seda",
                table: "ProductCategories",
                column: "ProductId",
                principalSchema: "Seda",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MeasureUnits_MeasureUnitId",
                schema: "Seda",
                table: "Products",
                column: "MeasureUnitId",
                principalSchema: "Gene",
                principalTable: "MeasureUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Cities_CityId",
                schema: "Seda",
                table: "Schools",
                column: "CityId",
                principalSchema: "Gene",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Zones_ZoneId",
                schema: "Seda",
                table: "Schools",
                column: "ZoneId",
                principalSchema: "Gene",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolUsers_AspNetUsers_ApplicationUserId",
                schema: "Seda",
                table: "SchoolUsers",
                column: "ApplicationUserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolUsers_Schools_SchoolId",
                schema: "Seda",
                table: "SchoolUsers",
                column: "SchoolId",
                principalSchema: "Seda",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Admi",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Admi",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTypeDers_Categories_CategoryId",
                schema: "Seda",
                table: "CategoryTypeDers");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTypeDers_CategoryTypes_CategoryTypeId",
                schema: "Seda",
                table: "CategoryTypeDers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                schema: "Seda",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                schema: "Seda",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_MeasureUnits_MeasureUnitId",
                schema: "Seda",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Cities_CityId",
                schema: "Seda",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Zones_ZoneId",
                schema: "Seda",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolUsers_AspNetUsers_ApplicationUserId",
                schema: "Seda",
                table: "SchoolUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolUsers_Schools_SchoolId",
                schema: "Seda",
                table: "SchoolUsers");

            migrationBuilder.DropTable(
                name: "SolicitDetails",
                schema: "Seda");

            migrationBuilder.DropTable(
                name: "Solicit",
                schema: "Seda");

            migrationBuilder.DropTable(
                name: "SolicitState",
                schema: "Seda");

            migrationBuilder.DropIndex(
                name: "IX_Zone_Name",
                schema: "Gene",
                table: "Zones");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Admi",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "Admi",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Admi",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "Admi",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "Admi",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTypeDers_Categories_CategoryId",
                schema: "Seda",
                table: "CategoryTypeDers",
                column: "CategoryId",
                principalSchema: "Seda",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTypeDers_CategoryTypes_CategoryTypeId",
                schema: "Seda",
                table: "CategoryTypeDers",
                column: "CategoryTypeId",
                principalSchema: "Seda",
                principalTable: "CategoryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Categories_CategoryId",
                schema: "Seda",
                table: "ProductCategories",
                column: "CategoryId",
                principalSchema: "Seda",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products_ProductId",
                schema: "Seda",
                table: "ProductCategories",
                column: "ProductId",
                principalSchema: "Seda",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MeasureUnits_MeasureUnitId",
                schema: "Seda",
                table: "Products",
                column: "MeasureUnitId",
                principalSchema: "Gene",
                principalTable: "MeasureUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Cities_CityId",
                schema: "Seda",
                table: "Schools",
                column: "CityId",
                principalSchema: "Gene",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schools_Zones_ZoneId",
                schema: "Seda",
                table: "Schools",
                column: "ZoneId",
                principalSchema: "Gene",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolUsers_AspNetUsers_ApplicationUserId",
                schema: "Seda",
                table: "SchoolUsers",
                column: "ApplicationUserId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolUsers_Schools_SchoolId",
                schema: "Seda",
                table: "SchoolUsers",
                column: "SchoolId",
                principalSchema: "Seda",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
