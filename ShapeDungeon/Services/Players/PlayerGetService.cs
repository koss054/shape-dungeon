using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Responses.Players;
using ShapeDungeon.Specifications.Players;

namespace ShapeDungeon.Services.Players
{
    public class PlayerGetService : IPlayerGetService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerGetService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<IEnumerable<PlayerGridDto>> GetAllPlayersAsync()
        {
            var players = await _playerRepository.GetMultipleByAsync(
                new PlayerAllSpecification());

            var playersDto = new List<PlayerGridDto>();

            foreach (var player in players) 
                playersDto.Add(player);

            return playersDto;
        }

        public async Task<PlayerDto> GetPlayerAsync(string name)
        {
            var player = await _playerRepository.GetFirstAsync(
                new PlayerNameSpecification(name));

            var playerDto = player;
            return playerDto;
        }

        public async Task<PlayerDto> GetActivePlayer()
            => await _playerRepository.GetFirstAsync(
                new PlayerIsActiveSpecification());

        public async Task<PlayerStatsResponse> GetActivePlayerStats()
            => await _playerRepository.GetFirstAsync(
                new PlayerIsActiveSpecification());
    }
}
