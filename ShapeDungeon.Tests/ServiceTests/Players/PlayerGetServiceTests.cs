using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Responses.Players;
using ShapeDungeon.Services.Players;
using ShapeDungeon.Specifications.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Players
{
    public class PlayerGetServiceTests
    {
        private readonly Mock<IPlayerRepository> _repoMock;
        private readonly IPlayerGetService _sut;
        private readonly IFixture _fixture;

        public PlayerGetServiceTests()
        {
            _repoMock = new Mock<IPlayerRepository>();
            _sut = new PlayerGetService(_repoMock.Object);
            _fixture = new Fixture();
        }

        [Theory]
        [MemberData(nameof(GetAllPlayersAsync_ValidItemsData))]
        public async Task GetAllPlayersAsync_ShouldReturnExpectedPlayersAsPlayerGridDto_WhenThereArePlayersInDb(
            int expectedCount, List<Player> expectedPlayers)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetMultipleByAsync(It.IsAny<PlayerAllSpecification>()))
                .ReturnsAsync(expectedPlayers);

            // Act
            var actualPlayers = await _sut.GetAllPlayersAsync();

            // Assert
            actualPlayers.Count().Should().Be(expectedCount);
            actualPlayers.FirstOrDefault().Should().BeOfType<PlayerGridDto>();
        }

        [Fact]
        public async Task GetAllPlayersAsync_ShouldReturnEmptyPlayerGridDtoList_WhenThereAreNoPlayersInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetMultipleByAsync(It.IsAny<PlayerAllSpecification>()))
                .ReturnsAsync(new List<Player>());

            // Act
            var actualPlayers = await _sut.GetAllPlayersAsync();

            // Assert
            actualPlayers.Count().Should().Be(0);
            actualPlayers.FirstOrDefault().Should().BeNull();
        }

        [Fact]
        public async Task GetPlayerAsync_ShouldReturnExpectedPlayerDto_WhenPlayerWithNameExistsInDb()
        {
            // Arrange
            var expectedName = "Eileen The Hoonter";

            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.Name, expectedName)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerNameSpecification>()))
                .ReturnsAsync(expectedPlayer);

            // Act
            var actualPlayer = await _sut.GetPlayerAsync(expectedName);

            // Assert
            actualPlayer.Should().NotBeNull();
            actualPlayer.Should().BeOfType<PlayerDto>();
            actualPlayer.Name.Should().Be(expectedName);
        }

        [Fact]
        public async Task GetPlayerAsync_ShouldThrowException_WhenNoPlayerMatchesProvidedNameInDb()
        {
            // Arrange
            var expectedName = "The Dude That Has a Katana in the Church and Can Teleport";

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerNameSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.GetPlayerAsync(expectedName);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        [Fact]
        public async Task GetActivePlayer_ShouldReturnPlayerDto_WhenThereIsAnActivePlayerInDb()
        {
            // Arrange
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            // Act
            var actualPlayer = await _sut.GetActivePlayer();

            // Assert
            actualPlayer.Should().BeOfType<PlayerDto>();
            actualPlayer.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task GetActivePlayer_ShouldThrowException_WhenNoActivePlayerInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.GetActivePlayer();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        [Fact]
        public async Task GetActivePlayerStats_ShouldReturnPlayerStatsResponse_WhenThereIsAnActivePlayerInDb()
        {
            // Arrange
            var expectedStrength = 10;
            var expectedVigor = 20;
            var expectedAgility = 30;

            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, true)
                .With(x => x.Strength, expectedStrength)
                .With(x => x.Vigor, expectedVigor)
                .With(x => x.Agility, expectedAgility)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            // Act
            var actualPlayer = await _sut.GetActivePlayerStats();

            // Assert
            actualPlayer.Should().BeOfType<PlayerStatsResponse>();
            actualPlayer.Strength.Should().Be(expectedStrength);
            actualPlayer.Vigor.Should().Be(expectedVigor);
            actualPlayer.Agility.Should().Be(expectedAgility);
        }

        [Fact]
        public async Task GetActivePlayerStats_ShouldThrowException_WhenNoActivePlayerInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.GetActivePlayerStats();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        public static IEnumerable<object[]> GetAllPlayersAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    3,                  // Expected Count
                    new List<Player>    // Expected Players
                    {
                        new Player{ Name = "Lol", IsActive = true },
                        new Player{ Name = "Bruh", IsActive = false },
                        new Player{ Name = "LolBruh", IsActive = false },
                    },
                },
                new object[]
                {
                    5,              // Expected Count
                    new List<Player> // Expected Players
                    {
                        new Player{ Name = "Incredibleu", IsActive = true },
                        new Player{ Name = "Neveroqtno", IsActive = false },
                        new Player{ Name = "Wunderbar", IsActive = true },
                        new Player{ Name = "Wunderbar", IsActive = true },
                        new Player{ Name = "Wunderbar", IsActive = false },
                    },
                }
            };
    }
}
