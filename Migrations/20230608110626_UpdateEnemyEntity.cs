using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class UpdateEnemyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentHp",
                table: "Enemies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentHp",
                table: "Enemies");
        }
    }
}
