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
    public class PlayerSelectServiceTests
    {
        private readonly Mock<IPlayerRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IPlayerSelectService _sut;
        private readonly IFixture _fixture;

        public PlayerSelectServiceTests()
        {
            _repoMock = new Mock<IPlayerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sut = new PlayerSelectService(_repoMock.Object, _unitOfWorkMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task UpdateActivePlayer_ShouldToggleIsActiveBetweenPlayers_WhenSpecificationsAreMet()
        {
            // Arrange
            var expectedNewPlayerId = Guid.NewGuid();

            var expectedOldPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, true)
                .Create();

            var expectedNewPlayer = _fixture.Build<Player>()
                .With(x => x.Id, expectedNewPlayerId)
                .With(x => x.IsActive, false)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedOldPlayer);

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIdSpecification>()))
                .ReturnsAsync(expectedNewPlayer);

            _repoMock.Object.Update(expectedOldPlayer);
            _repoMock.Object.Update(expectedNewPlayer);

            // Act
            await _sut.UpdateActivePlayer(expectedNewPlayerId);

            // Assert
            _repoMock.Verify(x => x.Update(expectedOldPlayer), Times.Once);
            _repoMock.Verify(x => x.Update(expectedNewPlayer), Times.Once);
            expectedOldPlayer.IsActive.Should().BeFalse();
            expectedNewPlayer.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateActivePlayer_ShouldThrowException_WhenNoActivePlayerInDb()
        {
            // Arrange
            var expectedNewPlayerId = Guid.NewGuid();
            var expectedNewPlayer = _fixture.Build<Player>()
                .With(x => x.Id, expectedNewPlayerId)
                .With(x => x.IsActive, false)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIdSpecification>()))
                .ReturnsAsync(expectedNewPlayer);

            // Act
            var action = async () => await _sut.UpdateActivePlayer(expectedNewPlayerId);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        [Fact]
        public async Task UpdateActivePlayer_ShouldThrowException_WhenNoPlayerWIthProvidedIdInDb()
        {
            // Arrange
            var expectedNewPlayerId = Guid.NewGuid();
            var expectedOldPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedOldPlayer);
                

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIdSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.UpdateActivePlayer(expectedNewPlayerId);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }
    }
}
