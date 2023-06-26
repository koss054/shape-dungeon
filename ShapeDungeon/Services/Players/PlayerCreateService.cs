using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Players
{
    public class PlayerCreateService : IPlayerCreateService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerCreateService( 
            IPlayerRepository playerRepository, 
            IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreatePlayerAsync(PlayerDto pDto)
        {
            var doesNameExist = await _playerRepository.DoesNameExist(pDto.Name);

            // If name exists a new player cannot be created.
            if (doesNameExist)
                return false;

            var player = new Player()
            {
                IsActive = false,
                Name = pDto.Name,
                Strength = pDto.Strength,
                Vigor = pDto.Vigor,
                Agility = pDto.Agility,
                Level = pDto.Strength + pDto.Vigor + pDto.Agility,
                CurrentExp = 0,
                ExpToNextLevel = 100,
                CurrentSkillpoints = 0,
                CurrentScoutEnergy = pDto.Agility,
                Shape = pDto.Shape
            };

            await _unitOfWork.Commit(() =>
            {
                _playerRepository.AddAsync(player);
            });

            doesNameExist = true;
            return doesNameExist;
        }
    }
}
