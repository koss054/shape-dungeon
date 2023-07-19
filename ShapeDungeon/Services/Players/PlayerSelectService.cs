using ShapeDungeon.Data;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Specifications.Players;

namespace ShapeDungeon.Services.Players
{
    public class PlayerSelectService : IPlayerSelectService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerSelectService(
            IPlayerRepository playerRepository, 
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateActivePlayer(Guid newActivePlayerId)
        {
            var oldActivePlayer = await _playerRepository.GetFirstAsync(
                new PlayerIsActiveSpecification());

            var newActivePlayer = await _playerRepository.GetFirstAsync(
                new PlayerIdSpecification(newActivePlayerId));

            oldActivePlayer.IsActive = false;
            newActivePlayer.IsActive = true;

            await _unitOfWork.Commit(() =>
            {
                _playerRepository.Update(oldActivePlayer);
                _playerRepository.Update(newActivePlayer);
            });
        }
    }
}
