#nullable disable
using Moq;
using NUnit.Framework;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Repos;
using ShapeDungeon.Services.Players;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShapeDungeon.Tests.ServiceTests.Players
{
    internal class PlayerGetServiceTests
    {
        private Mock<IPlayerRepositoryOld> _repoMock;
        private IPlayerGetService _service;

        [SetUp]
        public void Test_Initialize()
        {
            _repoMock = new Mock<IPlayerRepositoryOld>();
            _service = new PlayerGetService(_repoMock.Object);
        }

        [Test]
        public async Task GetAllPlayers_WithPlayersInDb_ReturnsAllPlayers()
        {
            // Arrange
            var playerList = new List<Player>();
            var firstName = "ThaSkull";
            var secondName = "ThaMami";
            var thirdName = "vanki4a";
            var fourthName = "Umfri";
            playerList.Add(new Player { Name = firstName });
            playerList.Add(new Player { Name = secondName });
            playerList.Add(new Player { Name = thirdName });
            playerList.Add(new Player { Name = fourthName });

            _repoMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(playerList);

            // Act
            var playerDtoList = await _service.GetAllPlayersAsync();

            // Assert
            Assert.IsNotNull(playerDtoList.FirstOrDefault(x => x.Name == firstName));
            Assert.IsNotNull(playerDtoList.FirstOrDefault(x => x.Name == secondName));
            Assert.IsNotNull(playerDtoList.FirstOrDefault(x => x.Name == thirdName));
            Assert.IsNotNull(playerDtoList.FirstOrDefault(x => x.Name == fourthName));
        }

        [Test]
        public async Task GetAllPlayers_WithNoPlayersInDb_ReturnsEmptyDtoList()
        {
            // Arrange

            // Act
            var playerDtoList = await _service.GetAllPlayersAsync();

            // Assert
            Assert.IsNull(playerDtoList.FirstOrDefault());
        }

        [Test]
        public async Task GetPlayer_WithExistingName_ReturnsExpectedPlayerDto()
        {
            // Arrange
            var existingName = "Skull054";
            var player = new Player { Name = existingName };

            _repoMock
                .Setup(x => x.GetByName(existingName))
                .ReturnsAsync(player);

            // Act
            var playerDto = await _service.GetPlayerAsync(existingName);

            // Assert
            Assert.IsNotNull(playerDto);
            Assert.AreEqual(playerDto.Name, existingName);
        }

        [Test]
        public async Task GetPlayer_WithMissingName_ReturnsEmptyPlayerDto()
        {
            // Arrange
            var missingName = "Skull055";

            // Act
            var playerDto = await _service.GetPlayerAsync(missingName);

            // Assert
            Assert.IsNull(playerDto.Name);
        }
    }
}
