using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class SQLiteChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enemies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Strength = table.Column<int>(type: "INTEGER", nullable: false),
                    Vigor = table.Column<int>(type: "INTEGER", nullable: false),
                    Agility = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    DroppedExp = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentHp = table.Column<int>(type: "INTEGER", nullable: false),
                    Shape = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enemies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    RequiredLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    BonusStrength = table.Column<int>(type: "INTEGER", nullable: false),
                    BonusVigor = table.Column<int>(type: "INTEGER", nullable: false),
                    BonusAgility = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Strength = table.Column<int>(type: "INTEGER", nullable: false),
                    Vigor = table.Column<int>(type: "INTEGER", nullable: false),
                    Agility = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentExp = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpToNextLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentSkillpoints = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentScoutEnergy = table.Column<int>(type: "INTEGER", nullable: false),
                    Shape = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsActiveForMove = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActiveForScout = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActiveForEdit = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanGoLeft = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanGoRight = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanGoUp = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanGoDown = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsStartRoom = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEnemyRoom = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsSafeRoom = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEndRoom = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnemyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CoordX = table.Column<int>(type: "INTEGER", nullable: false),
                    CoordY = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Enemies_EnemyId",
                        column: x => x.EnemyId,
                        principalTable: "Enemies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EnemiesRooms",
                columns: table => new
                {
                    EnemyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsEnemyDefeated = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnemiesRooms", x => new { x.EnemyId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_EnemiesRooms_Enemies_EnemyId",
                        column: x => x.EnemyId,
                        principalTable: "Enemies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnemiesRooms_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnemiesRooms_RoomId",
                table: "EnemiesRooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_EnemyId",
                table: "Rooms",
                column: "EnemyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnemiesRooms");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Enemies");
        }
    }
}
