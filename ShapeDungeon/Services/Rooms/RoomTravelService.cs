using ShapeDungeon.Data;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomTravelService : IRoomTravelService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomTravelService( 
            IRoomRepository roomRepository, 
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RoomTravelAsync(RoomDirection direction, RoomTravelAction action)
        {
            var oldRoom = action switch
            {
                RoomTravelAction.Move => await _roomRepository.GetActiveForMove(),
                RoomTravelAction.Scout => await _roomRepository.GetActiveForScout(),
                _ => throw new ArgumentOutOfRangeException(nameof(action)),
            };

            if (oldRoom == null) 
                throw new ArgumentNullException(nameof(oldRoom));

            var coordX = oldRoom!.CoordX;
            var coordY = oldRoom!.CoordY;

            switch (direction)
            {
                case RoomDirection.Left: 
                    var isLeftDeadEnd = !(await _roomRepository.CanEnterRoomFromDirection(coordX - 1, coordY, RoomDirection.Left));
                    if (oldRoom.CanGoLeft && !isLeftDeadEnd) coordX--;
                    break;
                case RoomDirection.Right:
                    var isRightDeadEnd = !(await _roomRepository.CanEnterRoomFromDirection(coordX + 1, coordY, RoomDirection.Right));
                    if (oldRoom.CanGoRight && !isRightDeadEnd) coordX++;  
                    break;
                case RoomDirection.Top: 
                    var isUpDeadEnd = !(await _roomRepository.CanEnterRoomFromDirection(coordX, coordY + 1, RoomDirection.Top));
                    if (oldRoom.CanGoUp && !isUpDeadEnd) coordY++; 
                    break;
                case RoomDirection.Bottom: 
                    var isDownDeadEnd = !(await _roomRepository.CanEnterRoomFromDirection(coordX, coordY - 1, RoomDirection.Bottom));
                    if (oldRoom.CanGoDown && !isDownDeadEnd) coordY--;  
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(direction));
            }

            var newRoom = await _roomRepository.GetByCoords(coordX, coordY);
            if (newRoom != null)
            {
                await _unitOfWork.Commit(() =>
                {
                    switch (action)
                    {
                        case RoomTravelAction.Move:
                            oldRoom.IsActiveForMove = false;
                            newRoom.IsActiveForMove = true;
                            break;
                        case RoomTravelAction.Scout:
                            oldRoom.IsActiveForScout = false;
                            newRoom.IsActiveForScout = true;
                            break;
                        default: throw new ArgumentOutOfRangeException(nameof(action));
                    }
                });
            }
        }

        public async Task<bool> IsScoutResetAsync()
        {
            var activeForScoutRoom = await _roomRepository.GetActiveForScout();
            var activeForMoveRoom = await _roomRepository.GetActiveForMove();

            if (activeForScoutRoom != null && activeForMoveRoom != null)
            {
                await _unitOfWork.Commit(() =>
                {
                    activeForScoutRoom.IsActiveForScout = false;
                    activeForMoveRoom.IsActiveForScout = true;
                });

                return true;
            }

            return false;
        }
    }
}
