using ShapeDungeon.Data;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Players
{
    public class PlayerCombatService : IPlayerCombatService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerCombatService(
            IPlayerRepository playerRepository, 
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExitCombat()
        {
            var player = await _playerRepository.GetActive();
            if (player != null)
            {
                await _unitOfWork.Commit(() =>
                {
                    player.IsInCombat = false;
                });
            }
        }
    }
}
