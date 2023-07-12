using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomActiveForEditService : IRoomActiveForEditService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRepositoryGet<Room> _roomRepositoryGet;
        private readonly IUnitOfWork _unitOfWork;

        public RoomActiveForEditService
            (IRoomRepository roomRepository,
            IRepositoryGet<Room> roomRepositoryGet,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _roomRepositoryGet = roomRepositoryGet;
            _unitOfWork = unitOfWork;
        }

        public async Task ApplyActiveForEditAsync(Guid roomId)
        {
            var rooms = await _roomRepositoryGet.GetAll();

            var oldRoom = _roomRepositoryGet.GetBy(
                new RoomTypeSpecification(RoomActiveType.Edit), rooms);

            if (oldRoom != null)
            {
                var newRoom = _roomRepositoryGet.GetBy(
                    new RoomIdSpecification(roomId), rooms);

                if (newRoom != null)
                    await ToggleActiveForEdit(oldRoom, newRoom);
            }
        }

        public async Task MoveActiveForEditAsync(int coordX, int coordY)
        {
            var oldRoom = await _roomRepository.GetActiveForEdit();
            if (oldRoom != null)
            {
                var newRoom = await _roomRepository.GetByCoords(coordX, coordY);
                if (newRoom != null)
                    await ToggleActiveForEdit(oldRoom, newRoom);
            }
        }

        private async Task ToggleActiveForEdit(Room oldRoom, Room newRoom)
        {
            oldRoom.IsActiveForEdit = false;
            newRoom.IsActiveForEdit = true;
            await _unitOfWork.Commit(() =>
            {
                _roomRepository.Update(oldRoom);
                _roomRepository.Update(newRoom);
            });
        }
    }
}
