using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Specifications.Players;

namespace ShapeDungeon.Services.Players
{
    public class PlayerUpdateService : IPlayerUpdateService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerUpdateService(
            IPlayerRepository playerRepository, 
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task IncreaseStat(CharacterStat statToIncrease)
        {
            var activePlayer = await GetActivePlayer();
            if (activePlayer.CurrentSkillpoints > 0)
            {
                switch (statToIncrease)
                {
                    case CharacterStat.Strength: activePlayer.Strength++; break;
                    case CharacterStat.Vigor: activePlayer.Vigor++; break;
                    case CharacterStat.Agility: activePlayer.Agility++; break;
                    default: throw new ArgumentOutOfRangeException(statToIncrease.ToString());
                }

                activePlayer.CurrentSkillpoints--;
                activePlayer.Level++;

                await _unitOfWork.Commit(() =>
                {
                    _playerRepository.Update(activePlayer);
                });
            }
        }

        // Move to player combat serivce.
        public async Task EnterCombat()
        {
            var activePlayer = await GetActivePlayer();
            activePlayer.IsInCombat = true;

            await _unitOfWork.Commit(() =>
            {
                _playerRepository.Update(activePlayer);
            });
        }

        public async Task LoseCombat()
        {
            var activePlayer = await GetActivePlayer();
            activePlayer.IsInCombat = false;
            activePlayer.CurrentExp = 0;

            await _unitOfWork.Commit(() =>
            {
                _playerRepository.Update(activePlayer);
            });
        }

        public async Task LevelUp()
        {
            var activePlayer = await GetActivePlayer();
            while (activePlayer.CurrentExp >= activePlayer.ExpToNextLevel)
            {
                activePlayer.CurrentExp -= activePlayer.ExpToNextLevel;
                activePlayer.CurrentSkillpoints++;
                activePlayer.ExpToNextLevel += 50;
            }

            await _unitOfWork.Commit(() =>
            {
                _playerRepository.Update(activePlayer);
            });
        }

        private async Task<Player> GetActivePlayer()
            => await _playerRepository.GetFirstAsync(
                new PlayerIsActiveSpecification());
    }
}
