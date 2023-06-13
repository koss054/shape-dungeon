using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class SeparateRoomPropIsActiveToMoveAndScout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Rooms",
                newName: "IsActiveForScout");

            migrationBuilder.AddColumn<bool>(
                name: "IsActiveForMove",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveForMove",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "IsActiveForScout",
                table: "Rooms",
                newName: "IsActive");
        }
    }
}
