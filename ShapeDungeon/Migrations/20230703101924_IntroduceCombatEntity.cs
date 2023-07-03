using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class IntroduceCombatEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Combats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentPlayerHp = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPlayerHp = table.Column<int>(type: "INTEGER", nullable: false),
                    EnemyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CurrentEnemyHp = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalEnemyHp = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Combats_Enemies_EnemyId",
                        column: x => x.EnemyId,
                        principalTable: "Enemies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Combats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Combats_EnemyId",
                table: "Combats",
                column: "EnemyId");

            migrationBuilder.CreateIndex(
                name: "IX_Combats_PlayerId",
                table: "Combats",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Combats");
        }
    }
}
