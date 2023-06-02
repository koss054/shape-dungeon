using ShapeDungeon.Data;
using ShapeDungeon.Interfaces.Entity;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomActiveForEditService : IRoomActiveForEditService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomActiveForEditService
            (IRoomRepository roomRepository, 
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ApplyActiveForEditAsync(Guid roomId)
        {
            var oldRoom = await _roomRepository.GetActiveForEdit();
            if (oldRoom != null)
            {
                var newRoom = await _roomRepository.GetById(roomId);
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

        private async Task ToggleActiveForEdit(IRoom oldRoom, IRoom newRoom)
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
