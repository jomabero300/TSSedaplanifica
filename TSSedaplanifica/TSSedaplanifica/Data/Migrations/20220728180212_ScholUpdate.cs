using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSSedaplanifica.Data.Migrations
{
    public partial class ScholUpdate : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitDetails_AspNetUsers_UserDeliveredId",
                schema: "Seda",
                table: "SolicitDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitDetails_Products_ProductId",
                schema: "Seda",
                table: "SolicitDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitDetails_Solicits_SolicitId",
                schema: "Seda",
                table: "SolicitDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_AspNetUsers_UserApprovedDeniedId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_AspNetUsers_UserClosedId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_AspNetUsers_UserReceivedId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_Schools_SchoolId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_SolicitState_SolicitStatesId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolUsers",
                schema: "Seda",
                table: "SchoolUsers");

            migrationBuilder.DropIndex(
                name: "IX_City_Zona_Scholl_ name",
                schema: "Seda",
                table: "Schools");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Seda",
                table: "SchoolUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolUsers",
                schema: "Seda",
                table: "SchoolUsers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUser_SchoolId_ApplicationUserId_ApplicationRole",
                schema: "Seda",
                table: "SchoolUsers",
                columns: new[] { "SchoolId", "ApplicationUserId", "ApplicationRole" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_name_DaneCode",
                schema: "Seda",
                table: "Schools",
                columns: new[] { "Name", "DaneCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_CityId",
                schema: "Seda",
                table: "Schools",
                column: "CityId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitDetails_AspNetUsers_UserDeliveredId",
                schema: "Seda",
                table: "SolicitDetails",
                column: "UserDeliveredId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitDetails_Products_ProductId",
                schema: "Seda",
                table: "SolicitDetails",
                column: "ProductId",
                principalSchema: "Seda",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitDetails_Solicits_SolicitId",
                schema: "Seda",
                table: "SolicitDetails",
                column: "SolicitId",
                principalSchema: "Seda",
                principalTable: "Solicits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_AspNetUsers_UserApprovedDeniedId",
                schema: "Seda",
                table: "Solicits",
                column: "UserApprovedDeniedId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_AspNetUsers_UserClosedId",
                schema: "Seda",
                table: "Solicits",
                column: "UserClosedId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_AspNetUsers_UserReceivedId",
                schema: "Seda",
                table: "Solicits",
                column: "UserReceivedId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_Schools_SchoolId",
                schema: "Seda",
                table: "Solicits",
                column: "SchoolId",
                principalSchema: "Seda",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_SolicitState_SolicitStatesId",
                schema: "Seda",
                table: "Solicits",
                column: "SolicitStatesId",
                principalSchema: "Seda",
                principalTable: "SolicitState",
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

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitDetails_AspNetUsers_UserDeliveredId",
                schema: "Seda",
                table: "SolicitDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitDetails_Products_ProductId",
                schema: "Seda",
                table: "SolicitDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitDetails_Solicits_SolicitId",
                schema: "Seda",
                table: "SolicitDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_AspNetUsers_UserApprovedDeniedId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_AspNetUsers_UserClosedId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_AspNetUsers_UserReceivedId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_Schools_SchoolId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicits_SolicitState_SolicitStatesId",
                schema: "Seda",
                table: "Solicits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolUsers",
                schema: "Seda",
                table: "SchoolUsers");

            migrationBuilder.DropIndex(
                name: "IX_SchoolUser_SchoolId_ApplicationUserId_ApplicationRole",
                schema: "Seda",
                table: "SchoolUsers");

            migrationBuilder.DropIndex(
                name: "IX_name_DaneCode",
                schema: "Seda",
                table: "Schools");

            migrationBuilder.DropIndex(
                name: "IX_Schools_CityId",
                schema: "Seda",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Seda",
                table: "SchoolUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolUsers",
                schema: "Seda",
                table: "SchoolUsers",
                columns: new[] { "SchoolId", "ApplicationUserId", "ApplicationRole" });

            migrationBuilder.CreateIndex(
                name: "IX_City_Zona_Scholl_ name",
                schema: "Seda",
                table: "Schools",
                columns: new[] { "CityId", "ZoneId", "Name" },
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

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitDetails_AspNetUsers_UserDeliveredId",
                schema: "Seda",
                table: "SolicitDetails",
                column: "UserDeliveredId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitDetails_Products_ProductId",
                schema: "Seda",
                table: "SolicitDetails",
                column: "ProductId",
                principalSchema: "Seda",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitDetails_Solicits_SolicitId",
                schema: "Seda",
                table: "SolicitDetails",
                column: "SolicitId",
                principalSchema: "Seda",
                principalTable: "Solicits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_AspNetUsers_UserApprovedDeniedId",
                schema: "Seda",
                table: "Solicits",
                column: "UserApprovedDeniedId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_AspNetUsers_UserClosedId",
                schema: "Seda",
                table: "Solicits",
                column: "UserClosedId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_AspNetUsers_UserReceivedId",
                schema: "Seda",
                table: "Solicits",
                column: "UserReceivedId",
                principalSchema: "Admi",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_Schools_SchoolId",
                schema: "Seda",
                table: "Solicits",
                column: "SchoolId",
                principalSchema: "Seda",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicits_SolicitState_SolicitStatesId",
                schema: "Seda",
                table: "Solicits",
                column: "SolicitStatesId",
                principalSchema: "Seda",
                principalTable: "SolicitState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
