using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Interfaces.Strategies;

// Not gonna lie, this "strategy" is a monstrosity, lol.
// Not sure if there's even a point of using it like this.
// Kinda overcompicates everything, but service smaller => unga happy.
namespace ShapeDungeon.Strategies.Updates.Rooms
{
    public class RoomSetNeighborsStrategy : IUpdateAsyncStrategy<RoomNavDto>
    {
        private readonly Task<bool> _isLeftValid;
        private readonly Task<bool> _isRightValid;
        private readonly Task<bool> _isUpValid;
        private readonly Task<bool> _isDownValid;
        private readonly Task<bool> _isLeftDeadEnd;
        private readonly Task<bool> _isRightDeadEnd;
        private readonly Task<bool> _isUpDeadEnd;
        private readonly Task<bool> _isDownDeadEnd;
        private RoomNavDto _roomDto;

        public RoomSetNeighborsStrategy(
            Task<bool> isLeftValid, 
            Task<bool> isRightValid, 
            Task<bool> isUpValid, 
            Task<bool> isDownValid, 
            Task<bool> isLeftDeadEnd, 
            Task<bool> isRightDeadEnd, 
            Task<bool> isUpDeadEnd, 
            Task<bool> isDownDeadEnd, 
            RoomNavDto roomDto)
        {
            _isLeftValid = isLeftValid;
            _isRightValid = isRightValid;
            _isUpValid = isUpValid;
            _isDownValid = isDownValid;
            _isLeftDeadEnd = isLeftDeadEnd;
            _isRightDeadEnd = isRightDeadEnd;
            _isUpDeadEnd = isUpDeadEnd;
            _isDownDeadEnd = isDownDeadEnd;
            _roomDto = roomDto;
        }

        public async Task<RoomNavDto> UpdateObject()
        {
            if (_roomDto.CanGoLeft)
            {
                _roomDto.HasLeftNeighbor = await _isLeftValid;
                _roomDto.IsLeftDeadEnd = !(await _isLeftDeadEnd);
            }

            if (_roomDto.CanGoRight)
            {
                _roomDto.HasRightNeighbor = await _isRightValid;
                _roomDto.IsRightDeadEnd = !(await _isRightDeadEnd);
            }

            if (_roomDto.CanGoUp)
            {
                _roomDto.HasUpNeighbor = await _isUpValid;
                _roomDto.IsUpDeadEnd = !(await _isUpDeadEnd);
            }

            if (_roomDto.CanGoDown)
            {
                _roomDto.HasDownNeighbor = await _isDownValid;
                _roomDto.IsDownDeadEnd = !(await _isDownDeadEnd);
            }

            return _roomDto;
        }
    }
}
