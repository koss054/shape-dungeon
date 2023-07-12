using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class UpdateEnemyRelatedEntitiesForCombat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomEnemyHealth",
                table: "EnemiesRooms");

            migrationBuilder.AddColumn<bool>(
                name: "IsActiveForCombat",
                table: "Enemies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveForCombat",
                table: "Enemies");

            migrationBuilder.AddColumn<int>(
                name: "RoomEnemyHealth",
                table: "EnemiesRooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
