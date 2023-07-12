using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomActiveForEditService : IRoomActiveForEditService
    {
        private readonly IRepositoryGet<Room> _roomGetRepository;
        private readonly IRepositoryUpdate<Room> _roomUpdateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomActiveForEditService(
            IRepositoryGet<Room> roomGetRepository,
            IRepositoryUpdate<Room> roomUpdateRepository,
            IUnitOfWork unitOfWork)
        {
            _roomGetRepository = roomGetRepository;
            _roomUpdateRepository = roomUpdateRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ApplyActiveForEditAsync(Guid roomId)
        {
            var rooms = await _roomGetRepository.GetAll();

            var oldRoom = _roomGetRepository.GetBy(
                new RoomTypeSpecification(RoomActiveType.Edit), rooms);

            if (oldRoom != null)
            {
                var newRoom = _roomGetRepository.GetBy(
                    new RoomIdSpecification(roomId), rooms);

                if (newRoom != null)
                    await ToggleActiveForEdit(oldRoom, newRoom);
            }
        }

        public async Task MoveActiveForEditAsync(int coordX, int coordY)
        {
            var rooms = await _roomGetRepository.GetAll();

            var oldRoom = _roomGetRepository.GetBy(
                new RoomTypeSpecification(RoomActiveType.Edit), rooms);

            if (oldRoom != null)
            {
                var newRoom = _roomGetRepository.GetBy(
                    new RoomCoordsSpecification(coordX, coordY), rooms);

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
                _roomUpdateRepository.Update(oldRoom);
                _roomUpdateRepository.Update(newRoom);
            });
        }
    }
}
