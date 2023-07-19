using ShapeDungeon.Data;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Players
{
    public class PlayerSelectService : IPlayerSelectService
    {
        private readonly IPlayerRepositoryOld _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerSelectService(
            IPlayerRepositoryOld playerRepository, 
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateActivePlayer(Guid newActivePlayerId)
        {
            var oldActivePlayer = await _playerRepository.GetActive();
            if (oldActivePlayer != null)
            {
                var newActivePlayer = await _playerRepository.GetById(newActivePlayerId);
                if (newActivePlayer != null)
                {
                    await _unitOfWork.Commit(() =>
                    {
                        oldActivePlayer.IsActive = false;
                        newActivePlayer.IsActive = true;
                    });
                }
            }
        }
    }
}
