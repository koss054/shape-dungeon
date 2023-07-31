using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;
using ShapeDungeon.Services.Players;
using ShapeDungeon.Specifications.Players;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Players
{
    public class PlayerCreateServiceTests
    {
        private readonly Mock<IPlayerRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IPlayerCreateService _sut;
        private readonly IFixture _fixture;

        public PlayerCreateServiceTests()
        {
            _repoMock = new Mock<IPlayerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sut = new PlayerCreateService(_repoMock.Object, _unitOfWorkMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreatePlayerAsync_ShouldReturnTrue_WhenPlayerCreatedSuccessfully()
        {
            // Arrange
            var uniqueName = "Minecraft Steve";

            var playerDto = _fixture.Build<PlayerDto>()
                .With(x => x.Name, uniqueName)
                .Create();

            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.Name, uniqueName)
                .Create();

            _repoMock
                .Setup(x => x.IsValidByAsync(It.IsAny<PlayerNameSpecification>()))
                .ReturnsAsync(false);

            await _repoMock.Object.AddAsync(expectedPlayer);

            // Act
            var actualBool = await _sut.CreatePlayerAsync(playerDto);

            // Assert
            _repoMock.Verify(x => x.AddAsync(It.IsAny<Player>()), Times.Once());
            actualBool.Should().BeTrue();
        }

        [Fact]
        public async Task CreatePlayerAsync_ShouldReturnFalse_WhenNewPlayerNameAlreadyExistsInDb()
        {
            // Arrange
            var duplicateName = "Steve";

            var playerDto = _fixture.Build<PlayerDto>()
                .With(x => x.Name, duplicateName)
                .Create();

            _repoMock
                .Setup(x => x.IsValidByAsync(It.IsAny<PlayerNameSpecification>()))
                .ReturnsAsync(true);

            // Act
            var actualBool = await _sut.CreatePlayerAsync(playerDto);

            // Assert
            _repoMock.Verify(x => x.AddAsync(It.IsAny<Player>()), Times.Never());
            actualBool.Should().BeFalse();
        }
    }
}
