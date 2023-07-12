using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Repos;
using ShapeDungeon.Responses.Players;

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
            var players = await _playerRepository.GetAll();
            var playersDto = new List<PlayerGridDto>();

            foreach (var player in players) 
                playersDto.Add(player);

            return playersDto;
        }

        public async Task<PlayerDto> GetPlayerAsync(string name)
        {
            var player = await _playerRepository.GetByName(name);
            var playerDto = new PlayerDto();

            if (player != null)
               playerDto = player;

            return playerDto;
        }

        public async Task<PlayerDto> GetActivePlayer()
            => await _playerRepository.GetActive();

        public async Task<PlayerStatsResponse> GetActivePlayerStats()
        {
            var player = await _playerRepository.GetActive();
            if (player == null) throw new ArgumentNullException(nameof(player));
            return player;
        }
    }
}
