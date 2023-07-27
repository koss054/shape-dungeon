using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Services.Players;
using ShapeDungeon.Specifications.Players;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Players
{
    public class PlayerScoutServiceTests
    {
        private readonly Mock<IPlayerRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IPlayerScoutService _sut;
        private readonly IFixture _fixture;

        public PlayerScoutServiceTests()
        {
            _repoMock = new Mock<IPlayerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sut = new PlayerScoutService(_repoMock.Object, _unitOfWorkMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetActiveScoutEnergyAsync_ShouldReturnActiveScoutEnergy_WhenThereIsAnActivePlayerInDb()
        {
            // Arrange
            var expectedScoutEnergy = 5;
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, true)
                .With(x => x.CurrentScoutEnergy, expectedScoutEnergy)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            // Act
            var actualScoutEnergy = await _sut.GetActiveScoutEnergyAsync();

            // Assert
            actualScoutEnergy.Should().Be(expectedScoutEnergy);
        }

        [Fact]
        public async Task GetActiveScoutEnergyAsync_ShouldThrowException_WhenThereIsNoActivePlayerInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.GetActiveScoutEnergyAsync();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        [Theory]
        [MemberData(nameof(UpdateActiveScoutEnergyAsync_ValidItemsData))]
        public async Task UpdateActiveScoutEnergyAsync_ShouldReturnExpectedUpdatedScoutEnergy_WhenAnActivePlayerIsInDb(
            int expectedScoutEnergy, Player expectedPlayer, PlayerScoutAction scoutAction)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            _repoMock.Object.Update(expectedPlayer);

            // Act
            var actualScoutEnergy = await _sut.UpdateActiveScoutEnergyAsync(scoutAction);

            // Assert
            _repoMock.Verify(x => x.Update(expectedPlayer), Times.Once());
            actualScoutEnergy.Should().Be(expectedScoutEnergy);
            expectedPlayer.CurrentScoutEnergy.Should().Be(expectedScoutEnergy);
        }

        [Fact]
        public async Task UpdateActiveScoutEnergyAsync_ShouldReturnMinusOne_WhenInvalidPlayerScoutActionIsPassed()
        {
            // Arrange
            var expectedResult = -1;
            var expectedAgility = 5;
            var expectedScoutEnergy = 5;
            var expectedScoutAction = (PlayerScoutAction)5;

            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.Agility, expectedAgility)
                .With(x => x.IsActive, true)
                .With(x => x.CurrentScoutEnergy, expectedScoutEnergy)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            // Act
            var actualResult = await _sut.UpdateActiveScoutEnergyAsync(expectedScoutAction);

            // Assert
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async Task UpdateActiveScoutEnergyAsync_ShouldReturnMinusOne_WhenReducingPlayerWithZeroCurrentScoutEnergy()
        {
            // Arrange
            var expectedResult = -1;
            var expectedAgility = 5;
            var expectedScoutEnergy = 0;
            var expectedScoutAction = PlayerScoutAction.Reduce;

            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.Agility, expectedAgility)
                .With(x => x.IsActive, true)
                .With(x => x.CurrentScoutEnergy, expectedScoutEnergy)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            // Act
            var actualResult = await _sut.UpdateActiveScoutEnergyAsync(expectedScoutAction);

            // Assert
            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [MemberData(nameof(UpdateActiveScoutEnergyAsync_ExceptionItemsData))]
        public async Task UpdateActiveScoutEnergyAsync_ShouldThrowException_WhenNoActivePlayerInDb(
            PlayerScoutAction scoutAction)
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.UpdateActiveScoutEnergyAsync(scoutAction);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        public static IEnumerable<object[]> UpdateActiveScoutEnergyAsync_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    5,                              // Expected Scout Energy
                    new Player                      // Player to Update
                    { 
                        IsActive = true, 
                        Agility = 5, 
                        CurrentScoutEnergy = 0,
                    },
                    PlayerScoutAction.Refill,       // Scout Update Action
                },
                new object[]
                {
                    4,                              // Expected Scout Energy
                    new Player                      // Player to Update
                    {
                        IsActive = true,
                        Agility = 10,
                        CurrentScoutEnergy = 5,
                    },
                    PlayerScoutAction.Reduce,       // Scout Update Action
                }
            };

        public static IEnumerable<object[]> UpdateActiveScoutEnergyAsync_ExceptionItemsData
            => new List<object[]>
            {
                new object[]
                {
                    PlayerScoutAction.Refill,       // Scout Update Action
                },
                new object[]
                {
                    PlayerScoutAction.Reduce,       // Scout Update Action
                }
            };
    }
}
