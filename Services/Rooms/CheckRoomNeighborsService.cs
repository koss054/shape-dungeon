using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Rooms
{
    public class CheckRoomNeighborsService : ICheckRoomNeighborsService
    {
        private readonly IRoomRepository _roomRepository;

        public CheckRoomNeighborsService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomNavDto?> SetDtoNeighborsAsync(int coordX, int coordY)
        {
            var currRoom = await InitializeCheckRoomAsync(coordX, coordY);

            if (currRoom != null)
            {
                if (currRoom.CanGoLeft)
                    currRoom.HasLeftNeighbor = await IsRoomWithCoordsValidAsync(coordX - 1, coordY);

                if (currRoom.CanGoRight)
                    currRoom.HasRightNeighbor = await IsRoomWithCoordsValidAsync(coordX + 1, coordY);

                if (currRoom.CanGoUp)
                    currRoom.HasUpNeighbor = await IsRoomWithCoordsValidAsync(coordX, coordY + 1);

                if (currRoom.CanGoDown)
                    currRoom.HasDownNeighbor = await IsRoomWithCoordsValidAsync(coordX, coordY - 1);
            }

            return currRoom;
        }

        public RoomDto SetHasNeighborsProperties(RoomDto room, RoomNavDto roomNav)
        {
            room.HasLeftNeighbor = roomNav.HasLeftNeighbor;
            room.HasRightNeighbor = roomNav.HasRightNeighbor;
            room.HasUpNeighbor = roomNav.HasUpNeighbor;
            room.HasDownNeighbor = roomNav.HasDownNeighbor;
            return room;
        }

        private async Task<RoomNavDto?> InitializeCheckRoomAsync(int coordX, int coordY)
        {
            var room = await _roomRepository.GetByCoords(coordX, coordY);
            var roomDto = new RoomNavDto();
            if (room != null)
            {
                roomDto.CoordX = room.CoordX;
                roomDto.CoordY = room.CoordY;
                roomDto.CanGoLeft = room.CanGoLeft;
                roomDto.CanGoRight = room.CanGoRight;
                roomDto.CanGoUp = room.CanGoUp;
                roomDto.CanGoDown = room.CanGoDown;
                roomDto.HasLeftNeighbor = false;
                roomDto.HasRightNeighbor = false;
                roomDto.HasUpNeighbor = false;
                roomDto.HasDownNeighbor = false;
            }

            return roomDto;
        }

        private async Task<bool> IsRoomWithCoordsValidAsync(int coordX, int coordY)
        {
            var room = await _roomRepository.GetByCoords(coordX, coordY);
            return room != null;
        }
    }
}
