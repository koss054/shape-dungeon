using ShapeDungeon.Data;
using ShapeDungeon.Entities;
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
            var player = await GetActive();
            await _unitOfWork.Commit(() =>
            {
                player.IsInCombat = false;
            });
        }

        public async Task GainExp(int gainedExp)
        {
            var player = await GetActive();
            await _unitOfWork.Commit(() =>
            {
                player.CurrentExp += gainedExp;
            });
        }

        private async Task<Player> GetActive()
        {
            var player = await _playerRepository.GetActive();
            if (player == null) throw new ArgumentNullException();
            return player;
        }
    }
}
