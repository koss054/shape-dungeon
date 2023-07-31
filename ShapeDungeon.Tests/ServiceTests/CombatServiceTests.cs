using AutoFixture;
using FluentAssertions;
using Moq;
using ShapeDungeon.Data;
using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Services;
using ShapeDungeon.Specifications.Combats;
using ShapeDungeon.Specifications.Enemies;
using ShapeDungeon.Specifications.EnemiesRooms;
using ShapeDungeon.Specifications.Players;
using ShapeDungeon.Specifications.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ShapeDungeon.Tests.ServiceTests
{
    public class CombatServiceTests
    {
        private readonly Mock<ICombatRepository> _combatRepoMock;
        private readonly Mock<IEnemyRepository> _enemyRepoMock;
        private readonly Mock<IPlayerRepository> _playerRepoMock;
        private readonly Mock<IRoomRepository> _roomRepoMock;
        private readonly Mock<IEnemyRoomRepository> _enemyRoomRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ICombatService _sut;
        private readonly IFixture _fixture;

        public CombatServiceTests()
        {
            _combatRepoMock = new Mock<ICombatRepository>();
            _enemyRepoMock = new Mock<IEnemyRepository>();
            _playerRepoMock = new Mock<IPlayerRepository>();
            _roomRepoMock = new Mock<IRoomRepository>();
            _enemyRoomRepoMock = new Mock<IEnemyRoomRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _fixture = new Fixture();
            _sut = new CombatService(
                _combatRepoMock.Object,
                _enemyRepoMock.Object,
                _playerRepoMock.Object,
                _roomRepoMock.Object,
                _enemyRoomRepoMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task InitializeCombat_ShouldThrowException_WhenNoActiveRoomInDb()
        {
            // Arrange
            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("roomToReturn", "No room matches provided specification."));

            // Act
            var action = async () => await _sut.InitializeCombat();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No room matches provided specification. (Parameter 'roomToReturn')");
        }

        [Fact]
        public async Task InitializeCombat_ShouldThrowException_WhenActiveRoomIsNotEnemyRoom()
        {
            // Arrange
            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.IsEnemyRoom, false)
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedRoom);

            // Act
            var action = async () => await _sut.InitializeCombat();

            // Assert
            await action.Should().ThrowAsync<Exception>()
                .WithMessage("NotEnemyRoomException");
        }

        [Fact]
        public async Task InitializeCombat_ShouldThrowException_WhenNoActivePlayerInDb()
        {
            // Arrange
            var isEnemyRoom = true;
            var isEnemyDefeated = false;

            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.IsEnemyRoom, isEnemyRoom)
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedRoom);

            _enemyRoomRepoMock
                .Setup(x => x.IsValidByAsync(It.IsAny<EnemyRoomDefeatedSpecification>()))
                .ReturnsAsync(isEnemyDefeated);

            _playerRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ThrowsAsync(new ArgumentNullException("playerToReturn", "No player matches provided specification."));

            // Act
            var action = async () => await _sut.InitializeCombat();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No player matches provided specification. (Parameter 'playerToReturn')");
        }

        [Fact]
        public async Task InitializeCombat_ShouldThrowException_WhenNoActiveForCombatEnemyInDb()
        {
            // Arrange
            var isActive = true;
            var isEnemyRoom = true;
            var isEnemyDefeated = false;

            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.IsEnemyRoom, isEnemyRoom)
                .Create();

            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, isActive)
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedRoom);

            _enemyRoomRepoMock
                .Setup(x => x.IsValidByAsync(It.IsAny<EnemyRoomDefeatedSpecification>()))
                .ReturnsAsync(isEnemyDefeated);

            _playerRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            _enemyRepoMock
                .Setup(X => X.GetFirstAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ThrowsAsync(new ArgumentNullException("enemyToReturn", "No enemy matches provided specification."));

            // Act
            var action = async () => await _sut.InitializeCombat();

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("No enemy matches provided specification. (Parameter 'enemyToReturn')");
        }

        // The combat repo exception can't be thrown if InitializeCombat passes successfully, and this method's exceptions are tested above.
        // Not testing the other exceptions because they are also tested above.
        [Fact]
        public async Task GetActiveCombat_ShouldReturnActiveCombat_WhenSpecificationsAreMet()
        {
            // Arrange
            var isActive = true;
            var isEnemyRoom = true;
            var isEnemyDefeated = false;
            var isActiveForCombat = true;

            var expectedRoom = _fixture.Build<Room>()
                .With(x => x.IsEnemyRoom, isEnemyRoom)
                .Create();

            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsActive, isActive)
                .Create();

            var expectedEnemy = _fixture.Build<Enemy>()
                .With(x => x.IsActiveForCombat, isActiveForCombat)
                .Create();

            var expectedCombat = _fixture.Build<Combat>()
                .With(x => x.IsActive, isActive)
                .With(x => x.PlayerId, expectedPlayer.Id)
                .With(x => x.Player, expectedPlayer)
                .With(x => x.EnemyId, expectedEnemy.Id)
                .With(x => x.Enemy, expectedEnemy)
                .Create();

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedRoom);

            _enemyRoomRepoMock
                .Setup(x => x.IsValidByAsync(It.IsAny<EnemyRoomDefeatedSpecification>()))
                .ReturnsAsync(isEnemyDefeated);

            _playerRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<PlayerIsActiveSpecification>()))
                .ReturnsAsync(expectedPlayer);

            _enemyRepoMock
                .Setup(X => X.GetFirstAsync(It.IsAny<EnemyActiveForCombatSpecification>()))
                .ReturnsAsync(expectedEnemy);

            _combatRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<CombatIsActiveSpecification>()))
                .ReturnsAsync(expectedCombat);

            // Act
            var actualCombat = await _sut.GetActiveCombat();

            // Assert
            actualCombat.Should().BeOfType<CombatDto>();
            actualCombat.Player.Name.Should().Be(expectedPlayer.Name);
            actualCombat.Enemy.Name.Should().Be(expectedEnemy.Name);
        }

        [Fact]
        public async Task HasPlayerWon_ShouldReturnTrue_WhenPlayerWins()
        {
            // Arrange
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsInCombat, true)
                .With(x => x.IsActive, true)
                .Create();

            var expectedEnemyRoom = _fixture.Build<EnemyRoom>()
                .With(x => x.IsEnemyDefeated, false)
                .Create();

            var expectedCombat = _fixture.Build<Combat>()
                .With(x => x.IsActive, true)
                .With(x => x.PlayerId, expectedPlayer.Id)
                .With(x => x.Player, expectedPlayer)
                .With(x => x.CurrentEnemyHp, 0)
                .With(x => x.CombatRoomId, expectedEnemyRoom.RoomId)
                .Create();

            _combatRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<CombatIsActiveSpecification>()))
                .ReturnsAsync(expectedCombat);

            _enemyRoomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyRoomIdSpecification>()))
                .ReturnsAsync(expectedEnemyRoom);

            _enemyRoomRepoMock.Object.Update(expectedEnemyRoom);
            _combatRepoMock.Object.Update(expectedCombat);

            // Act
            var actualResult = await _sut.HasPlayerWon();

            // Assert
            actualResult.Should().BeTrue();
            expectedEnemyRoom.IsEnemyDefeated.Should().BeTrue();
            expectedCombat.IsActive.Should().BeFalse();
            expectedPlayer.IsInCombat.Should().BeFalse();
        }

        [Fact]
        public async Task HasPlayerWon_ShouldReturnFalse_WhenPlayerLoses()
        {
            // Arrange
            var expectedPlayer = _fixture.Build<Player>()
                .With(x => x.IsInCombat, true)
                .With(x => x.IsActive, true)
                .With(x => x.CurrentExp, 200)
                .Create();

            var expectedEnemyRoom = _fixture.Build<EnemyRoom>()
                .With(x => x.IsEnemyDefeated, false)
                .Create();

            var expectedCombat = _fixture.Build<Combat>()
                .With(x => x.IsActive, true)
                .With(x => x.PlayerId, expectedPlayer.Id)
                .With(x => x.Player, expectedPlayer)
                .With(x => x.CurrentEnemyHp, 10)
                .With(x => x.CombatRoomId, expectedEnemyRoom.RoomId)
                .Create();

            var expectedCombatRoom = _fixture.Build<Room>()
                .With(x => x.Id, expectedEnemyRoom.RoomId)
                .With(x => x.IsActiveForMove, true)
                .With(x => x.IsActiveForScout, false)
                .Create();

            var expectedPrevRoom = _fixture.Build<Room>()
                .With(x => x.IsActiveForScout, true)
                .With(x => x.IsActiveForEdit, false)
                .Create();

            var expectedStartRoom = _fixture.Build<Room>()
                .With(x => x.CoordX, 0)
                .With(x => x.CoordY, 0)
                .With(x => x.IsStartRoom, true)
                .With(x => x.IsActiveForMove, false)
                .With(x => x.IsActiveForScout, false)
                .Create();

            _combatRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<CombatIsActiveSpecification>()))
                .ReturnsAsync(expectedCombat);

            _enemyRoomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<EnemyRoomIdSpecification>()))
                .ReturnsAsync(expectedEnemyRoom);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomMoveSpecification>()))
                .ReturnsAsync(expectedCombatRoom);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomScoutSpecification>()))
                .ReturnsAsync(expectedPrevRoom);

            _roomRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<RoomCoordsSpecification>()))
                .ReturnsAsync(expectedStartRoom);

            _roomRepoMock.Object.Update(expectedCombatRoom);
            _roomRepoMock.Object.Update(expectedPrevRoom);
            _roomRepoMock.Object.Update(expectedStartRoom);
            _combatRepoMock.Object.Update(expectedCombat);

            // Act
            var actualResult = await _sut.HasPlayerWon();

            // Assert
            actualResult.Should().BeFalse();
            expectedEnemyRoom.IsEnemyDefeated.Should().BeFalse();
            expectedCombat.IsActive.Should().BeFalse();
            expectedPlayer.IsInCombat.Should().BeFalse();
            expectedCombatRoom.IsActiveForMove.Should().BeFalse();
            expectedCombatRoom.IsActiveForScout.Should().BeFalse();
            expectedPrevRoom.IsActiveForScout.Should().BeFalse();
            expectedPrevRoom.IsActiveForMove.Should().BeFalse();
            expectedStartRoom.IsActiveForMove.Should().BeTrue();
            expectedStartRoom.IsActiveForScout.Should().BeTrue();
            expectedPlayer.CurrentExp.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(IsPlayerAttackingInActiveCombat_ValidItemsData))]
        public async Task IsPlayerAttackingInActiveCombat_ShouldReturnExpectedResult_WhenActiveCombatInDb(
            bool expectedResult, Combat expectedCombat)
        {
            // Arrange
            _combatRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<CombatIsActiveSpecification>()))
                .ReturnsAsync(expectedCombat);

            // Act
            var actualResult = await _sut.IsPlayerAttackingInActiveCombat();

            // Assert
            actualResult.Should().Be(expectedResult);
        }

        [Theory]
        [MemberData(nameof(ToggleIsPlayerAttackingInActiveCombat_ValidItemsData))]
        public async Task ToggleIsPlayerAttackingInActiveCombat_ShouldReturnExpectedResult_WhenActiveCombatInDb(
            bool expectedResult, Combat expectedCombat)
        {
            // Arrange
            _combatRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<CombatIsActiveSpecification>()))
                .ReturnsAsync(expectedCombat);

            _combatRepoMock.Object.Update(expectedCombat);

            // Act
            await _sut.ToggleIsPlayerAttackingInActiveCombat();

            // Assert
            expectedCombat.IsPlayerAttacking.Should().Be(expectedResult);
        }

        [Theory]
        [MemberData(nameof(UpdateHealthAfterAttack_ValidItemsData))]
        public async Task UpdateHealthAfterAttack_ShouldReturnExpectedResult_WhenValidCharacterType(
            int expectedHp, int hpLost, int expectedPlayerHp, int expectedEnemyHp, CombatCharacterType expectedType, Combat expectedCombat)
        {
            // Arrange
            _combatRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<CombatIsActiveSpecification>()))
                .ReturnsAsync(expectedCombat);

            _combatRepoMock.Object.Update(expectedCombat);

            // Act
            var actualHealth = await _sut.UpdateHealthAfterAttack(hpLost, (int)expectedType);

            // Assert
            actualHealth.Should().Be(expectedHp);
            expectedCombat.CurrentPlayerHp.Should().Be(expectedPlayerHp);
            expectedCombat.CurrentEnemyHp.Should().Be(expectedEnemyHp);
        }

        [Fact]
        public async Task UpdateHealthAfterAttack_ShouldThrowException_WhenInvalidCharacterType()
        {
            // Arrange
            var expectedType = 222;

            var expectedCombat = _fixture.Build<Combat>()
                .With(x => x.IsActive, true)
                .Create();

            _combatRepoMock
                .Setup(x => x.GetFirstAsync(It.IsAny<CombatIsActiveSpecification>()))
                .ReturnsAsync(expectedCombat);

            // Act
            var action = async () => await _sut.UpdateHealthAfterAttack(It.IsAny<int>(), expectedType);

            // Assert
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("Specified argument was out of the range of valid values. (Parameter 'characterType')");
        }

        public static IEnumerable<object[]> IsPlayerAttackingInActiveCombat_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    true,       // Expected Result
                    new Combat  // Expected Combat
                    {
                        IsPlayerAttacking = true,
                    }
                },
                new object[]
                {
                    false,      // Expected Result
                    new Combat  // Expected Combat
                    {
                        IsPlayerAttacking = false,
                    }
                },
            };

        public static IEnumerable<object[]> ToggleIsPlayerAttackingInActiveCombat_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    false,      // Expected Result
                    new Combat  // Expected Combat
                    {
                        IsPlayerAttacking = true,
                    }
                },
                new object[]
                {
                    true,       // Expected Result
                    new Combat  // Expected Combat
                    {
                        IsPlayerAttacking = false,
                    }
                },
            };

        public static IEnumerable<object[]> UpdateHealthAfterAttack_ValidItemsData
            => new List<object[]>
            {
                new object[]
                {
                    15,                 // Expected Return Hp                         
                    5,                  // Hp Lost
                    15,                 // Expected Player Hp
                    20,                 // Expected Enemy Hp
                    CombatCharacterType // Character Type
                    .Player,
                    new Combat          // Expected Combat
                    {
                        IsActive = true,
                        CurrentPlayerHp = 20,
                        CurrentEnemyHp = 20,
                        IsPlayerAttacking = false,
                    }
                },
                new object[]
                {
                    5,                  // Expected Return Hp                         
                    25,                 // Hp Lost
                    20,                 // Expected Player Hp
                    5,                  // Expected Enemy Hp
                    CombatCharacterType // Character Type
                    .Enemy,
                    new Combat          // Expected Combat
                    {
                        IsActive = true,
                        CurrentPlayerHp = 20,
                        CurrentEnemyHp = 30,
                        IsPlayerAttacking = true,
                    }
                },
            };
    }
}
