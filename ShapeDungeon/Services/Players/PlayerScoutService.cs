using ShapeDungeon.Data;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Specifications.Players;

namespace ShapeDungeon.Services.Players
{
    public class PlayerScoutService : IPlayerScoutService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerScoutService(
            IPlayerRepository playerRepository, 
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetActiveScoutEnergyAsync()
        {
            var activePlayer = await _playerRepository.GetFirstAsync(
                new PlayerIsActiveSpecification());

            return activePlayer.CurrentScoutEnergy;
        }

        public async Task<int> UpdateActiveScoutEnergyAsync(PlayerScoutAction action)
        {
            var currActivePlayer = await _playerRepository.GetFirstAsync(
                new PlayerIsActiveSpecification());

            var currScoutEnergy = currActivePlayer.CurrentScoutEnergy;

            if (action == PlayerScoutAction.Reduce && currScoutEnergy > 0)
                currScoutEnergy--;
            else if (action == PlayerScoutAction.Refill)
                currScoutEnergy = currActivePlayer.Agility;
            else
                return -1;

            currActivePlayer.CurrentScoutEnergy = currScoutEnergy;

            await _unitOfWork.Commit(() =>
            {
                _playerRepository.Update(currActivePlayer);
            });

            return currScoutEnergy;
        }
    }
}
