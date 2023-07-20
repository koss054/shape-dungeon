using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Specifications.Players;
using ShapeDungeon.Strategies.Creational;

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
            var doesNameExist = await _playerRepository.IsValidByAsync(
                new PlayerNameSpecification(pDto.Name));

            // If name exists a new player cannot be created.
            if (doesNameExist)
                return false;

            var playerCreateContext = new CreateContext<Player, PlayerDto>(
                new PlayerCreateStrategy(pDto));

            var player = playerCreateContext.ExecuteStrategy();

            await _unitOfWork.Commit(() =>
            {
                _playerRepository.AddAsync(player);
            });

            doesNameExist = true;
            return doesNameExist;
        }
    }
}
