using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSSedaplanifica.Data.Migrations
{
    public partial class AddSchoolEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Zones",
                schema: "Gene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                schema: "Seda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DaneCode = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false),
                    Address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Gene",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schools_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "Gene",
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolImages",
                schema: "Seda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: true),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolImages_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "Seda",
                        principalTable: "Schools",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SchoolUsers",
                schema: "Seda",
                columns: table => new
                {
                    ApplicationRole = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HireOfDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndOfDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isEnable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolUsers", x => new { x.SchoolId, x.ApplicationUserId, x.ApplicationRole });
                    table.ForeignKey(
                        name: "FK_SchoolUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Admi",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolUsers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalSchema: "Seda",
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolImages_SchoolId",
                schema: "Seda",
                table: "SchoolImages",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_City_Zona_Scholl_ name",
                schema: "Seda",
                table: "Schools",
                columns: new[] { "CityId", "ZoneId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_ZoneId",
                schema: "Seda",
                table: "Schools",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUsers_ApplicationUserId",
                schema: "Seda",
                table: "SchoolUsers",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolImages",
                schema: "Seda");

            migrationBuilder.DropTable(
                name: "SchoolUsers",
                schema: "Seda");

            migrationBuilder.DropTable(
                name: "Schools",
                schema: "Seda");

            migrationBuilder.DropTable(
                name: "Zones",
                schema: "Gene");
        }
    }
}
