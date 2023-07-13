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
            var oldRoom = await _roomGetRepository.GetFirstOrDefaultByAsync(
                new RoomTypeSpecification(RoomActiveType.Edit));

            if (oldRoom != null)
            {
                var newRoom = await _roomGetRepository.GetFirstOrDefaultByAsync(
                    new RoomIdSpecification(roomId));

                if (newRoom != null)
                    await ToggleActiveForEdit(oldRoom, newRoom);
            }
        }

        public async Task MoveActiveForEditAsync(int coordX, int coordY)
        {
            var oldRoom = await _roomGetRepository.GetFirstOrDefaultByAsync(
                new RoomTypeSpecification(RoomActiveType.Edit));

            if (oldRoom != null)
            {
                var newRoom = await _roomGetRepository.GetFirstOrDefaultByAsync(
                    new RoomCoordsSpecification(coordX, coordY));

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
