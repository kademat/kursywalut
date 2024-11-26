using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NbpTables",
                columns: table => new
                {
                    No = table.Column<string>(type: "TEXT", nullable: false),
                    EffectiveDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Table = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NbpTables", x => x.No);
                });

            migrationBuilder.CreateTable(
                name: "NbpRate",
                columns: table => new
                {
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Mid = table.Column<decimal>(type: "TEXT", nullable: false),
                    NbpTableNo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NbpRate", x => x.Code);
                    table.ForeignKey(
                        name: "FK_NbpRate_NbpTables_NbpTableNo",
                        column: x => x.NbpTableNo,
                        principalTable: "NbpTables",
                        principalColumn: "No");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NbpRate_NbpTableNo",
                table: "NbpRate",
                column: "NbpTableNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NbpRate");

            migrationBuilder.DropTable(
                name: "NbpTables");
        }
    }
}
