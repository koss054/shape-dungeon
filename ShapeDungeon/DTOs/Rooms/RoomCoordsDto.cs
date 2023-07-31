namespace ShapeDungeon.DTOs.Rooms
{
    public class RoomCoordsDto
    {
        public RoomCoordsDto()
        {

        }

        public RoomCoordsDto(int coordX, int coordY)
        {
            CoordX = coordX;
            CoordY = coordY;
        }

        public int CoordX { get; set; }
        public int CoordY { get; set; }
    }
}
