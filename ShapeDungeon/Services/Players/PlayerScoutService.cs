using ShapeDungeon.Data;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Repos;

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
            var currScoutEnergy = await _playerRepository.GetActiveScoutEnergy();
            return currScoutEnergy ?? 0;
        }

        public async Task<int> UpdateActiveScoutEnergyAsync(PlayerScoutAction action)
        {
            var currActivePlayer = await _playerRepository.GetActive();
            if (currActivePlayer == null)
                throw new NullReferenceException(nameof(currActivePlayer));

            var currScoutEnergy = currActivePlayer.CurrentScoutEnergy;

            if (action == PlayerScoutAction.Reduce && currScoutEnergy > 0)
                currScoutEnergy--;
            else if (action == PlayerScoutAction.Refill)
                currScoutEnergy = currActivePlayer.Agility;
            else
                return -1;

            await _unitOfWork.Commit(() =>
            {
                currActivePlayer.CurrentScoutEnergy = currScoutEnergy;
            });

            return currScoutEnergy;
        }
    }
}
