using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Player;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerService( 
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
                Level = 1,
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

            doesNameExist = await _playerRepository.DoesNameExist(pDto.Name);
            return doesNameExist;
        }

        public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
        {
            var players = await _playerRepository.GetAll();
            var playersDto = new List<PlayerDto>();

            foreach (var player in players)
            {
                var playerDto = new PlayerDto()
                {
                    IsActive = player.IsActive,
                    Name = player.Name,
                    Strength = player.Strength,
                    Vigor = player.Vigor,
                    Agility = player.Agility,
                    Level = player.Level,
                    CurrentExp = player.CurrentExp,
                    ExpToNextLevel = player.ExpToNextLevel,
                    CurrentSkillpoints = player.CurrentSkillpoints,
                    CurrentScoutEnergy = player.CurrentScoutEnergy,
                    Shape = player.Shape,
                };

                playersDto.Add(playerDto);
            }

            return playersDto;
        }

        public async Task<PlayerDto?> GetPlayerAsync(string name)
        {
            var player = await _playerRepository.GetByName(name);
            var playerDto = new PlayerDto();

            if (player != null)
            {
               playerDto.IsActive = player.IsActive;
               playerDto.Name = player.Name;
               playerDto.Strength = player.Strength;
               playerDto.Vigor = player.Vigor;
               playerDto.Agility = player.Agility;
               playerDto.Level = player.Level;
               playerDto.CurrentExp = player.CurrentExp;
               playerDto.ExpToNextLevel = player.ExpToNextLevel;
               playerDto.CurrentSkillpoints = player.CurrentSkillpoints;
               playerDto.CurrentScoutEnergy = player.CurrentScoutEnergy;
               playerDto.Shape = player.Shape;
            }

            return playerDto;
        }
    }
}
