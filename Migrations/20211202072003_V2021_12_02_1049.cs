using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePgExercises.Migrations
{
    public partial class V2021_12_02_1049 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    FacId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "0, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MemberCost = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    GuestCost = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    InitialOutlay = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    MonthlyMaintenance = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.FacId);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "0, 1"),
                    Surname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RecommendedBy = table.Column<int>(type: "int", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemId);
                    table.ForeignKey(
                        name: "FK_Members_Members_RecommendedBy",
                        column: x => x.RecommendedBy,
                        principalTable: "Members",
                        principalColumn: "MemId");
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "0, 1"),
                    FacId = table.Column<int>(type: "int", nullable: false),
                    MemId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Slots = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Bookings_Facilities_FacId",
                        column: x => x.FacId,
                        principalTable: "Facilities",
                        principalColumn: "FacId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Members_MemId",
                        column: x => x.MemId,
                        principalTable: "Members",
                        principalColumn: "MemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_facid_starttime",
                table: "Bookings",
                columns: new[] { "FacId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_memid_facid",
                table: "Bookings",
                columns: new[] { "MemId", "FacId" });

            migrationBuilder.CreateIndex(
                name: "IX_memid_starttime",
                table: "Bookings",
                columns: new[] { "MemId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_starttime",
                table: "Bookings",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_JoinDate",
                table: "Members",
                column: "JoinDate");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedBy",
                table: "Members",
                column: "RecommendedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
