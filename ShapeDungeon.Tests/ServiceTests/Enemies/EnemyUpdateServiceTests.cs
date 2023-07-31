using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Services.Enemies;
using ShapeDungeon.Specifications.Enemies;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests.Enemies
{
    public class EnemyUpdateServiceTests
    {
        private readonly Mock<IEnemyRepository> _repoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IEnemyUpdateService _sut;
        private readonly IFixture _fixture;

        public EnemyUpdateServiceTests()
        {
            _repoMock = new Mock<IEnemyRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _sut = new EnemyUpdateService(_repoMock.Object, _unitOfWorkMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task RemoveActiveForCombat_ShouldChangeIsActiveForCombatToFalse_WhenActiveForCombatEnemyIsInDb()
        {
            // Arrange
            var expectedEnemy = _fixture.Build<Enemy>()
                .With(x => x.IsActiveForCombat, true)
                .Create();

            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ReturnsAsync(expectedEnemy);

            _repoMock.Object.Update(expectedEnemy);

            // Act
            await _sut.RemoveActiveForCombat();

            // Assert
            _repoMock.Verify(x => x.Update(It.IsAny<Enemy>()), Times.Once());
            expectedEnemy.IsActiveForCombat.Should().BeFalse();
        }

        [Fact]
        public async Task RemoveActiveForCombat_ShouldThrowException_WhenNoActiveForCombatEnemyIsInDb()
        {
            // Arrange
            _repoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ThrowsAsync(new ArgumentNullException("enemyToReturn", "No enemy matches provided specification."));

            // Act
            var action = async () => await _sut.RemoveActiveForCombat();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy matches provided specification. (Parameter 'enemyToReturn')");
        }
    }
}
