using ShapeDungeon.Data;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Repos;

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

        // Haduken code created lol
        // After error handling middleware is implemented this will be fixed, hopefully :D
        public async Task IncreaseStat(CharacterStat statToIncrease)
        {
            var activePlayer = await _playerRepository.GetActive();
            if (activePlayer != null)
            {
                if (activePlayer.CurrentSkillpoints > 0)
                {
                    await _unitOfWork.Commit(() =>
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
                    });
                }
            }
        }
    }
}
