using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Strategies;

namespace ShapeDungeon.Strategies.Creational
{
    public class PlayerCreateStrategy : ICreateStrategy<Player, PlayerDto>
    {
        private readonly PlayerDto _playerDto;

        public PlayerCreateStrategy(PlayerDto playerDto)
        {
            _playerDto = playerDto;
        }

        public Player CreateObject()
        {
            var player = new Player()
            {
                IsActive = false,
                IsInCombat = false,
                Name = _playerDto.Name,
                Strength = _playerDto.Strength,
                Vigor = _playerDto.Vigor,
                Agility = _playerDto.Agility,
                Level = _playerDto.Strength + _playerDto.Vigor + _playerDto.Agility,
                CurrentExp = 0,
                ExpToNextLevel = 100,
                CurrentSkillpoints = 0,
                CurrentScoutEnergy = _playerDto.Agility,
                Shape = _playerDto.Shape
            };

            return player;
        }
    }
}
