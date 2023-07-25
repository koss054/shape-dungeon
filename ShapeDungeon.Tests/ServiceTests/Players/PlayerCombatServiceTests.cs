using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Services.Players;
using ShapeDungeon.Specifications.Players;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Players
{
    public class PlayerCombatServiceTests
    {
        private readonly Mock<IPlayerRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IPlayerCombatService _sut;
        private readonly IFixture _fixture;

        public PlayerCombatServiceTests()
        {
            _repoMock = new Mock<IPlayerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sut = new PlayerCombatService(_repoMock.Object, _unitOfWorkMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ExitCombat_ShouldChangeIsInCombatToFalse_WhenThereIsAnActivePlayerInDb()
        {
            // Arrange
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, true)
                .With(x => x.IsInCombat, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            _repoMock.Object.Update(expectedPlayer);

            // Act
            await _sut.ExitCombat();

            // Assert
            _repoMock.Verify(x => x.Update(It.IsAny<Player>()), Times.Once());
            expectedPlayer.IsActive.Should().BeTrue();
            expectedPlayer.IsInCombat.Should().BeFalse();
        }

        [Fact]
        public async Task ExitCombat_ShouldThrowException_WhenThereIsNoActivePlayerInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.ExitCombat();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");

        }

        [Fact]
        public async Task GainExp_ShouldIncreasePlayerExp_WhenThereIsAnActivePlayerInDb()
        {
            // Arrange
            var expectedExpGain = 64;
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, true)
                .With(x => x.CurrentExp, 0)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            _repoMock.Object.Update(expectedPlayer);

            // Act
            await _sut.GainExp(expectedExpGain);

            // Assert
            _repoMock.Verify(x => x.Update(It.IsAny<Player>()), Times.Once());
            expectedPlayer.CurrentExp.Should().Be(expectedExpGain);
        }

        [Fact]
        public async Task GainExp_ShouldThrowException_WhenThereIsNoActivePlayerInDb()
        {
            // Arrange
            var expectedExpGain = 54;

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.GainExp(expectedExpGain);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");

        }
    }
}
