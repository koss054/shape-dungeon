using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Helpers.Enums;
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
                {
                    currRoom.HasLeftNeighbor = await IsRoomWithCoordsValidAsync(coordX - 1, coordY);
                    currRoom.IsLeftDeadEnd = !(await CanEnterRoomWithCoordsFromDirection(coordX - 1, coordY, RoomDirection.Left));
                }

                if (currRoom.CanGoRight)
                {
                    currRoom.HasRightNeighbor = await IsRoomWithCoordsValidAsync(coordX + 1, coordY);
                    currRoom.IsRightDeadEnd = !(await CanEnterRoomWithCoordsFromDirection(coordX + 1, coordY, RoomDirection.Right));
                }

                if (currRoom.CanGoUp)
                {
                    currRoom.HasUpNeighbor = await IsRoomWithCoordsValidAsync(coordX, coordY + 1);
                    currRoom.IsUpDeadEnd = !(await CanEnterRoomWithCoordsFromDirection(coordX, coordY + 1, RoomDirection.Top));
                }

                if (currRoom.CanGoDown)
                {
                    currRoom.HasDownNeighbor = await IsRoomWithCoordsValidAsync(coordX, coordY - 1);
                    currRoom.IsDownDeadEnd = !(await CanEnterRoomWithCoordsFromDirection(coordX, coordY - 1, RoomDirection.Bottom));
                }
            }

            return currRoom;
        }

        public RoomDto SetHasNeighborsProperties(RoomDto room, RoomNavDto roomNav)
        {
            room.HasLeftNeighbor = roomNav.HasLeftNeighbor;
            room.HasRightNeighbor = roomNav.HasRightNeighbor;
            room.HasUpNeighbor = roomNav.HasUpNeighbor;
            room.HasDownNeighbor = roomNav.HasDownNeighbor;
            room.IsLeftDeadEnd = roomNav.IsLeftDeadEnd;
            room.IsRightDeadEnd = roomNav.IsRightDeadEnd;
            room.IsUpDeadEnd = roomNav.IsUpDeadEnd;
            room.IsDownDeadEnd = roomNav.IsDownDeadEnd;
            return room;
        }

        private async Task<RoomNavDto?> InitializeCheckRoomAsync(int coordX, int coordY)
        {
            var room = await _roomRepository.GetByCoords(coordX, coordY);
            var roomDto = new RoomNavDto();
            if (room != null)
            {
                roomDto = room;
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

        private async Task<bool> CanEnterRoomWithCoordsFromDirection(int coordX, int coordY, RoomDirection direction)
        {
            var room = await _roomRepository.GetByCoords(coordX, coordY);
            var canGo = false;

            if (room != null)
            {
                switch (direction)
                {
                    case RoomDirection.Left: canGo = room.CanGoRight; break;
                    case RoomDirection.Right: canGo = room.CanGoLeft;  break;
                    case RoomDirection.Top: canGo = room.CanGoDown;  break;
                    case RoomDirection.Bottom: canGo = room.CanGoUp; break;
                    default: throw new ArgumentOutOfRangeException(nameof(direction));
                }
            }

            return canGo;
        }
    }
}
