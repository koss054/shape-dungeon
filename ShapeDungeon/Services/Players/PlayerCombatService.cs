using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Specifications.Players;

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
            player.IsInCombat = false;

            await _unitOfWork.Commit(() =>
            {
                _playerRepository.Update(player);
            });
        }

        public async Task GainExp(int gainedExp)
        {
            var player = await GetActive();
            player.CurrentExp += gainedExp;

            await _unitOfWork.Commit(() =>
            {
                _playerRepository.Update(player);
            });
        }

        private async Task<Player> GetActive()
        {
            var player = await _playerRepository.GetFirstAsync(
                new PlayerIsActiveSpecification());

            return player;
        }
    }
}
