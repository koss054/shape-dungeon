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
                    if (oldRoom.CanGoLeft)
                        coordX--;  
                    break;
                case RoomDirection.Right: 
                    if (oldRoom.CanGoRight)
                        coordX++;  
                    break;
                case RoomDirection.Top: 
                    if (oldRoom.CanGoUp)
                        coordY++; 
                    break;
                case RoomDirection.Bottom: 
                    if (oldRoom.CanGoDown)
                        coordY--;  
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

        public async Task ResetScoutAsync()
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
            }
        }
    }
}
